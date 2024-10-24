using PetApp.Data;
using PetApp.DbConnection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PetApp
{
    public partial class PetsListPage : Page
    {
        private List<Pet> originalPets;

        public PetsListPage(List<Pet> pets)
        {
            InitializeComponent();
            originalPets = pets;
            UpdatePetsList(originalPets);
        }

        private void UpdatePetsList(List<Pet> pets)
        {
            PetsListView.ItemsSource = pets;
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = SortComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                var pets = originalPets.ToList();
                switch (selectedItem.Content.ToString())
                {
                    case "Кличка (А-Я)":
                        pets = pets.OrderBy(p => p.klichka).ToList();
                        break;
                    case "Кличка (Я-А)":
                        pets = pets.OrderByDescending(p => p.klichka).ToList();
                        break;
                    case "Тип (А-Я)":
                        pets = pets.OrderBy(p => p.type).ToList();
                        break;
                    case "Тип (Я-А)":
                        pets = pets.OrderByDescending(p => p.type).ToList();
                        break;
                }
                UpdatePetsList(pets);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            var filteredPets = originalPets.Where(p => p.klichka.ToLower().Contains(searchText) || p.type.ToLower().Contains(searchText)).ToList();
            UpdatePetsList(filteredPets);
        }
    }
}