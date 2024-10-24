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
using Microsoft.VisualBasic; // Добавьте это пространство имен

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
                    break;
                case 2:
                    UserImage.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    UserImage.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }

            // Подписываемся на событие изменения выбора в ComboBox
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
            // Открываем диалог выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Загружаем выбранное изображение
                var photoPath = openFileDialog.FileName;
                var newPhoto = new BitmapImage(new System.Uri(photoPath));

                // Добавляем фотографию в соответствующий список в зависимости от выбранного питомца
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

                // Обновляем отображение фотографий
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

            // Открываем диалог для ввода описания
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

            // Открываем новое окно для отображения фотографии и её описания
            var photoViewerWindow = new PhotoViewerWindow(petPhoto.Image, petPhoto.Description);
            photoViewerWindow.ShowDialog();
        }

        private void ShowPetsListButton_Click(object sender, RoutedEventArgs e)
        {
            var pets = DataBaseManager.GetAllPets();
            var petsListPage = new PetsListPage(pets);
            ContentFrame.Navigate(petsListPage);
            HeaderTextBlock.Visibility = Visibility.Collapsed;
            ShowPetsListButton.Visibility = Visibility.Collapsed; // Скрываем кнопку "Список питомцев"
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
                HeaderTextBlock.Visibility = Visibility.Visible;
                ShowPetsListButton.Visibility = Visibility.Visible; // Показываем кнопку "Список питомцев"
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
            // Обновляем видимость заголовка в зависимости от текущей страницы
            if (ContentFrame.Content is PetsListPage)
            {
                HeaderTextBlock.Visibility = Visibility.Collapsed;
                ShowPetsListButton.Visibility = Visibility.Collapsed; // Скрываем кнопку "Список питомцев"
            }
            else
            {
                HeaderTextBlock.Visibility = Visibility.Visible;
                ShowPetsListButton.Visibility = Visibility.Visible; // Показываем кнопку "Список питомцев"
            }
        }

        private void PetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePetPhotos();
        }
    }
}