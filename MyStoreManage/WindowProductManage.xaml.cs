using Microsoft.EntityFrameworkCore;
using MyStoreManage.Models;
using MyStoreManage.session_login;
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
using System.Windows.Shapes;

namespace MyStoreManage
{
    /// <summary>
    /// Interaction logic for WindowProductManage.xaml
    /// </summary>
    public partial class WindowProductManage : Window
    {
        private readonly MyStoreContext _context;
        public WindowProductManage(MyStoreContext context)
        {
            InitializeComponent();
            _context = context;
            HandleBeforeLoadGirdView();
            HandleComboboxCategory();
            HandleStaffNameNavigate();
        }
        public void HandleStaffNameNavigate()
        {
            txblStaffNameNavigate.Text = SessionService.Instance.GetNameInSession();
        }
        public void HandleComboboxCategory()
        {
            cbbCategory.ItemsSource = _context.Categories.ToList();
        }
        public void HandleBeforeLoadGirdView()
        {
            lvProducts.ItemsSource = _context.Products.Include(x => x.Category).ToList();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                if (cbbCategory.SelectedItem == null)
                {
                    MessageBox.Show("Please select a category before inserting a product.");
                    return;
                }

                var newProduct = new Product
                {
                    ProductName = txtProductName.Text,

                    CategoryId = ((Category)cbbCategory.SelectedItem).CategoryId,
                    UnitPrice = int.Parse(txtUnitPrice.Text)

                };

                _context.Products.Add(newProduct);
                _context.SaveChanges();

                HandleBeforeLoadGirdView();
                MessageBox.Show("Product inserted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting product: {ex.Message}");
            }


        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvProducts.SelectedItem != null)
            {
                var selectedProduct = (Product)lvProducts.SelectedItem;
                selectedProduct.ProductName = txtProductName.Text;
                selectedProduct.UnitPrice = int.Parse(txtUnitPrice.Text);
                selectedProduct.CategoryId = ((Category)cbbCategory.SelectedItem).CategoryId;
                try
                {
                    _context.Products.Update(selectedProduct);
                    _context.SaveChanges();
                    HandleBeforeLoadGirdView();
                    MessageBox.Show("Update Successfully", "Notification");


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating product: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a product to update", "Notification");
            }

        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvProducts.SelectedItem != null)
            {
                var selectedProduct = (Product)lvProducts.SelectedItem;
                selectedProduct.ProductName = txtProductName.Text;

                MessageBoxResult result = MessageBox.Show(
                     $"Are you sure you want to delete the category '{selectedProduct.ProductName}'?",
                     "Confirm Delete",
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _context.Products.Remove(selectedProduct);
                        _context.SaveChanges();
                        HandleBeforeLoadGirdView();
                        MessageBox.Show("Delete Successfully", "Notification");


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error delete product: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete", "Notification");
            }

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string productName = txtSearch.Text.Trim();
                string unitPriceText = txtSearch_Copy.Text.Trim();
                int? unitPrice = null;


                if (!string.IsNullOrWhiteSpace(unitPriceText))
                {
                    unitPrice = int.Parse(unitPriceText);
                }

                var query = _context.Products
                    .Include(x => x.Category)
                    .AsQueryable();


                if (!string.IsNullOrWhiteSpace(productName) && unitPrice.HasValue)
                {
                    query = query.Where(p => p.ProductName.Contains(productName) && p.UnitPrice == unitPrice);
                }
                else
                {

                    if (!string.IsNullOrWhiteSpace(productName))
                    {
                        query = query.Where(p => p.ProductName.Contains(productName));
                    }

                    if (unitPrice.HasValue)
                    {
                        query = query.Where(p => p.UnitPrice == unitPrice);
                    }
                }


                lvProducts.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching products: {ex.Message}");
            }
        }
        //ToolBar
        private void btnOpenCategoriesManage_Click(object sender, RoutedEventArgs e)
        {
            var windowCategoryManage = new WindowCategoryManage(_context);
            this.Close();
            windowCategoryManage.Show();
            e.Handled = true;
        }

        private void btnOpenOrdersManage_Click(object sender, RoutedEventArgs e)
        {
            var windowOrderManage = new WindowOrderManage(_context);
            this.Close();
            windowOrderManage.Show();
            e.Handled = true;
        }

        private void btnOpenStaffManage_Click(object sender, RoutedEventArgs e)
        {
            var windowStaffManage = new WindowStaffManage(_context);
            this.Close();
            windowStaffManage.Show();
            e.Handled = true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionService.Instance.ClearSession();
            var mainWindow = new MainWindow(_context);
            this.Close();
            mainWindow.Show();
            e.Handled = true;
        }

        private void btnOpenMyAccount_Click(object sender, RoutedEventArgs e)
        {
            var WindowMyProfile = new WindowMyProfile(_context);
            this.Close();
            WindowMyProfile.Show();
            e.Handled = true;
        }

        //private void SearchProduct()
        //{
        //    try
        //    {
        //        string keyword = txtSearch.Text;
        //        lvProducts.ItemsSource = _context.Products
        //            .Include(x => x.Category)
        //            .Where(p => p.ProductName.Contains(keyword))
        //            .ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error searching products: {ex.Message}");
        //    }
        //}
        //private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    SearchProduct();
        //}
    }
}
