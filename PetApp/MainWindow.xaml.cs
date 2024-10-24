using Microsoft.Win32;
using PetApp.Data;
using PetApp.Windows;
using PetApp.DbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic; 

namespace PetApp
{
    public partial class MainWindow : Window
    {
        private List<PetPhoto> raPhotos = new List<PetPhoto>();
        private List<PetPhoto> nubiPhotos = new List<PetPhoto>();

        public MainWindow()
        {
            InitializeComponent();
            UpdateWelcomeText();
            LoadPetInfo();

            switch (UserContext.AuthUser.id_user)
            {
                case 1:
                    UserImage.Visibility = Visibility.Visible;
                    User1Image.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    UserImage.Visibility = Visibility.Collapsed;
                    User1Image.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }

            PetComboBox.SelectionChanged += PetComboBox_SelectionChanged;
        }

        private void UpdateWelcomeText()
        {
            if (UserContext.AuthUser != null)
            {
                WelcomeText.Text = $"Добро пожаловать, {UserContext.AuthUser.username}";
            }
        }

        private void LoadPetInfo()
        {
            var pet = DataBaseManager.GetPetByUserId(UserContext.AuthUser.id_user);
            if (pet != null)
            {
                PetTypeTextBlock.Text = $"Тип: {pet.type}";
                PetKlichkaTextBlock.Text = $"Кличка: {pet.klichka}";
            }
            else
            {
                PetTypeTextBlock.Text = "Тип: Нет питомца";
                PetKlichkaTextBlock.Text = "Кличка: Нет питомца";
            }
        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                var photoPath = openFileDialog.FileName;
                var newPhoto = new BitmapImage(new System.Uri(photoPath));

                if (PetComboBox.SelectedItem != null)
                {
                    var selectedPet = PetComboBox.SelectedItem as ComboBoxItem;
                    if (selectedPet.Content.ToString() == "Ра")
                    {
                        raPhotos.Add(new PetPhoto { Image = newPhoto });
                    }
                    else if (selectedPet.Content.ToString() == "Нуби")
                    {
                        nubiPhotos.Add(new PetPhoto { Image = newPhoto });
                    }
                }

                UpdatePetPhotos();
            }
        }

        private void UpdatePetPhotos()
        {
            PetPhotoPanel.Children.Clear();
            var photos = PetComboBox.SelectedItem != null && (PetComboBox.SelectedItem as ComboBoxItem).Content.ToString() == "Ра" ? raPhotos : nubiPhotos;
            foreach (var petPhoto in photos)
            {
                var image = new Image
                {
                    Source = petPhoto.Image,
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(5)
                };
                image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
                image.Tag = petPhoto;

                var button = new Button
                {
                    Content = "Добавить описание",
                    Width = 120,
                    Height = 30,
                    Margin = new Thickness(5),
                    Tag = petPhoto
                };
                button.Click += AddDescriptionButton_Click;

                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                stackPanel.Children.Add(image);
                stackPanel.Children.Add(button);

                PetPhotoPanel.Children.Add(stackPanel);
            }
        }

        private void AddDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var petPhoto = button.Tag as PetPhoto;

            var description = Interaction.InputBox("Введите описание фотографии:", "Описание фотографии", petPhoto.Description);
            if (!string.IsNullOrEmpty(description))
            {
                petPhoto.Description = description;
                MessageBox.Show("Описание добавлено!");
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            var petPhoto = image.Tag as PetPhoto;

            var photoViewerWindow = new PhotoViewerWindow(petPhoto.Image, petPhoto.Description);
            photoViewerWindow.ShowDialog();
        }

        private void ShowPetsListButton_Click(object sender, RoutedEventArgs e)
        {
            var pets = DataBaseManager.GetAllPets();
            var petsListPage = new PetsListPage(pets);
            ContentFrame.Navigate(petsListPage);
            HeaderTextBlock.Visibility = Visibility.Collapsed;
            ShowPetsListButton.Visibility = Visibility.Collapsed; 
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
                HeaderTextBlock.Visibility = Visibility.Visible;
                ShowPetsListButton.Visibility = Visibility.Visible; 
            }
        }

        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoForward)
            {
                ContentFrame.GoForward();
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (ContentFrame.Content is PetsListPage)
            {
                HeaderTextBlock.Visibility = Visibility.Collapsed;
                ShowPetsListButton.Visibility = Visibility.Collapsed; 
            }
            else
            {
                HeaderTextBlock.Visibility = Visibility.Visible;
                ShowPetsListButton.Visibility = Visibility.Visible; 
            }
        }

        private void PetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePetPhotos();
        }
    }
}