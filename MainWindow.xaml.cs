﻿using MalinoviyUgarBiblioteka;
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
using System.Xml.Linq;

namespace LibWPF
{
    public partial class MainWindow : Window
    {
        private static List<User> users = new List<User>()
        {
            new User(1,"Прав","Алексей"),
            new User(2,"Неправ","Владимир"),
            new User(3,"Качеля", "Василий"),
        };
        private static List<Book> books = new List<Book>()
        {
            new Book("зимби", 12345, new DateOnly(1992,12,1),2),
            new Book("крутыш", 21212, new DateOnly(2044,4,1),400),
            new Book("абобус", 31321, new DateOnly(2077,6,7),240),
            new Book("амогус", 12321, new DateOnly(4230,10,1),1),
        };
        public MainWindow()
        {
            InitializeComponent();

            ReadersList.ItemsSource = users;
            BooksList.ItemsSource = books;
            ReadersAddList.ItemsSource = users;
            BooksAddList.ItemsSource = books;
        }
        private void BooksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book? books = BooksList.SelectedItem as Book;
            if (books != null)
            {
                authorText.Text = Convert.ToString(books.Author);
                vendorCodeText.Text = Convert.ToString(books.VendorCode);
                yearText.Text = Convert.ToString(books.Year);
                amountText.Text = Convert.ToString(books.Amount);
            }
        }
        private void ReaderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User? selectedUser = ReadersList.SelectedItem as User;
            if (selectedUser != null)
            {
                idText.Text = Convert.ToString(selectedUser.Id);
                nameText.Text = Convert.ToString(selectedUser.Name);
                surnameText.Text = Convert.ToString(selectedUser.Surname);
                foreach (var item in selectedUser.Books)
                {
                    BooksText.Text = Convert.ToString(selectedUser.Books.Count);
                }
            }
        }
        private void AddReader_Click(object sender, RoutedEventArgs e)
        {
            if (users != null)
            {
                int newId;
                if (int.TryParse(idText.Text, out newId))
                {
                    string newName = nameText.Text;
                    string newSurname = surnameText.Text;
                    User newUser = new User(newId, newName, newSurname);
                    users.Add(newUser);
                    idText.Text = "";
                    nameText.Text = "";
                    surnameText.Text = "";
                    ReadersList.ItemsSource = users;
                    ReadersList.Items.Refresh();
                    ReadersAddList.Items.Refresh();
                    MessageBox.Show("Пользователь успешно добавлен.");
                }
            }
            else MessageBox.Show("Введите значение ID");
        }
        private void DeleteReader_Click(object sender, RoutedEventArgs e)
        {
            if (users != null && ReadersList.SelectedItem != null)
            {
                User? selectedUser = ReadersList.SelectedItem as User;
                MessageBoxResult res = MessageBox.Show($"Удалить пользователя {selectedUser.Name} " +
                    $"{selectedUser.Surname}?", "Подтвердить", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {

                    users.Remove(selectedUser);
                    ReadersList.ItemsSource = users;
                    ReadersList.Items.Refresh();
                    ReadersAddList.Items.Refresh();
                    MessageBox.Show("Пользователь удалён");
                }
            }
        }
        private void RerideReader_Click(object sender, RoutedEventArgs e)
        {
            if (ReadersList.SelectedItem != null)
            {
                User? selecterUser = ReadersList.SelectedItem as User;
                if (selecterUser != null)
                {
                    selecterUser.Name = nameText.Text;
                    selecterUser.Surname = surnameText.Text;
                    ReadersList.Items.Refresh();
                    ReadersAddList.Items.Refresh();
                    MessageBox.Show("Данные пользователя обновлены");
                }
            }
        }
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            if (books != null)
            {
                string newAuthor = authorText.Text;
                int newVendorCode;
                {
                    if (int.TryParse(vendorCodeText.Text, out newVendorCode))
                    {
                        DateOnly newYear;
                        if (DateOnly.TryParse(yearText.Text, out newYear))
                        {
                            int newAmount;
                            if (int.TryParse(amountText.Text, out newAmount))
                            {
                                Book newBook = new Book(newAuthor, newVendorCode, newYear, newAmount);
                                books.Add(newBook);
                                authorText.Text = "";
                                vendorCodeText.Text = "";
                                yearText.Text = "";
                                amountText.Text = "";
                                BooksList.ItemsSource = books;
                                BooksList.Items.Refresh();
                                BooksAddList.Items.Refresh();
                                MessageBox.Show("Книга успешно добавлена");
                            }
                            else MessageBox.Show("Введите числовое значение количества книг");
                        }
                        else MessageBox.Show("Введите дату в формате: YYYY, MM, DD");
                    }
                    else MessageBox.Show("Введите числовое значение для артикула");
                }
            }
        }
        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (books != null && BooksList.SelectedItem != null)
            {
                Book? selectedBook = BooksList.SelectedItem as Book;
                books.Remove(selectedBook);
                BooksList.ItemsSource = books;
                BooksList.Items.Refresh();
                BooksAddList.Items.Refresh();
                MessageBox.Show("Книга удалена");
            }
        }
        private void RerideBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksList.SelectedItem != null)
            {
                Book? selectedBook = BooksList.SelectedItem as Book;
                if (selectedBook != null)
                {
                    selectedBook.Author = authorText.Text;
                    selectedBook.VendorCode = Convert.ToInt32(vendorCodeText.Text);
                    selectedBook.Year = DateOnly.Parse(yearText.Text);
                    selectedBook.Amount = Convert.ToInt32(amountText.Text);
                    BooksList.Items.Refresh();
                    BooksAddList.Items.Refresh();
                    MessageBox.Show("Информация о книге обновлена");
                }
            }
        }
        private void AddBookToUser(object sender, RoutedEventArgs e)
        {
            User? selectedUser = ReadersAddList.SelectedItem as User;
            Book? selectedBook = BooksAddList.SelectedItem as Book;

            if (selectedUser == null || selectedBook == null)
            {
                MessageBox.Show("Не выбран читатель или книга");
                return;
            }
            if (selectedBook.Amount > 0)
            {
                selectedBook.Amount--;
                selectedUser.Books.Add(selectedBook);
                ReadersAddList.Items.Refresh();
                BooksAddList.Items.Refresh();
                ReadersList.Items.Refresh();
                BooksList.Items.Refresh();
                ReadersAddList.SelectedItem = null;
                BooksAddList.SelectedItem = null;
                MessageBox.Show("Книга выдана читателю");
            }
            else MessageBox.Show("Недостаточно экземпляров книги для выдачи");
        }

    }
}
