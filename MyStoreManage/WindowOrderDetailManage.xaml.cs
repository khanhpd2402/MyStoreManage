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
    /// Interaction logic for WindowOrderDetailManage.xaml
    /// </summary>
    public partial class WindowOrderDetailManage : Window
    {
        private readonly MyStoreContext _storeContext;
        List<Product> products;
        List<OrderDetail> orderDetails;
        Order _selectedOrder;
        public WindowOrderDetailManage(MyStoreContext storeContext, Order selectedOrder)

        {
            InitializeComponent();
            _storeContext = storeContext;
            _selectedOrder = selectedOrder;
            HandleStaffNameNavigate();
            HandleComboboxProduct();
            HandleGridView();
            HandleButttonRole();
        }
        public void HandleStaffNameNavigate()
        {
            txblStaffNameNavigate.Text = SessionService.Instance.GetNameInSession();
        }

        public void HandleComboboxProduct()
        {
            products = _storeContext.Products.ToList();
            cbbProductName.ItemsSource = products;
        }
        public void HandleGridView()
        {
            var orderId = _selectedOrder.OrderId;
            orderDetails = _storeContext.OrderDetails.Where(x => x.OrderId == orderId).ToList();
            lvOrderDetail.ItemsSource = orderDetails;
        }
        private void ShowAllOrderDetails()
        {
            List<OrderDetail> orderDetails = GetAllOrderDetails();
            if (orderDetails != null)
            {
                lvOrderDetail.ItemsSource = orderDetails; // Gán danh sách chi tiết đơn hàng vào ItemsSource của ListView
            }
            else
            {
                lvOrderDetail.ItemsSource = null; // Đặt ItemsSource thành null nếu không có chi tiết đơn hàng nào
            }
        }
        private void UpdateOrderDetailList()
        {
            lvOrderDetail.ItemsSource = GetAllOrderDetails();
        }

        private List<OrderDetail> GetAllOrderDetails()
        {
            try
            {
                return _storeContext.OrderDetails.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy danh sách chi tiết đơn hàng từ cơ sở dữ liệu!");
                return new List<OrderDetail>(); // Trả về danh sách rỗng nếu có lỗi
            }
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            var orderDetail = GetOrderDetailObject();
            if (orderDetail != null)
            {
                orderDetail.OrderId = _selectedOrder.OrderId;
                try
                {
                    if (orderDetail != null)
                    {
                        _storeContext.OrderDetails.Add(orderDetail);
                        _storeContext.SaveChanges();
                        HandleGridView();
                        MessageBox.Show("Thêm chi tiết đơn hàng thành công!", "Thêm chi tiết đơn hàng");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể thêm chi tiết đơn hàng!", "Thêm chi tiết đơn hàng");
                }
            }
        }
        private OrderDetail GetOrderDetailObject()
        {
            try
            {
                int productId = (cbbProductName.SelectedItem as Product).ProductId;

                var product = _storeContext.Products.FirstOrDefault(p => p.ProductId == productId);
                if (productId == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!", "Thêm chi tiết đơn hàng", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }

                //decimal unitPrice;
                //if (!decimal.TryParse(txtUnitPrice.Text, out unitPrice) || unitPrice <= 0)
                //{
                //    MessageBox.Show("Vui lòng nhập giá tiền hợp lệ!", "Thêm chi tiết đơn hàng", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return null;
                //}
                decimal unitPrice = product.UnitPrice;

                int quantity;
                if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thêm chi tiết đơn hàng", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }



                return new OrderDetail
                {
                    ProductId = productId,
                    UnitPrice = (int)unitPrice,
                    Quantity = quantity,
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy chi tiết đơn hàng: " + ex.Message, "Thêm chi tiết đơn hàng", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        public void HandleButttonRole()
        {
            var role = SessionService.Instance.GetRoleInSession();
            if (role == 0)
            {
                btnOpenOrdersManage.Visibility = Visibility.Hidden;
            }
            else if (role == 1)
            {
                btnOpenProductsManage.Visibility = Visibility.Hidden;
                btnOpenCategoriesManage.Visibility = Visibility.Hidden;
                btnOpenStaffManage.Visibility = Visibility.Hidden;
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDetail = lvOrderDetail.SelectedItem as OrderDetail;
            if (selectedOrderDetail != null)
            {
                try
                {

                    selectedOrderDetail.Quantity = int.Parse(txtQuantity.Text);
                    selectedOrderDetail.UnitPrice = (int)decimal.Parse(txtUnitPrice.Text);

                    _storeContext.Entry<OrderDetail>(selectedOrderDetail).State = EntityState.Modified;
                    _storeContext.SaveChanges();
                    UpdateOrderDetailList();
                    HandleGridView();
                    MessageBox.Show("Cập nhật chi tiết đơn hàng thành công!", "Cập nhật chi tiết đơn hàng");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể cập nhật chi tiết đơn hàng!", "Cập nhật chi tiết đơn hàng");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một chi tiết đơn hàng để cập nhật!", "Cập nhật chi tiết đơn hàng");
            }
        }
        private void lvOrderDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrderDetail = lvOrderDetail.SelectedItem as OrderDetail;
            if (selectedOrderDetail != null)
            {
                txtProductID.Text = selectedOrderDetail.ProductId.ToString();
                cbbProductName.Text = selectedOrderDetail.Product.ProductName;

                txtQuantity.Text = selectedOrderDetail.Quantity.ToString();
                txtUnitPrice.Text = selectedOrderDetail.UnitPrice.ToString();
            }
        }



        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDetail = lvOrderDetail.SelectedItem as OrderDetail;
            if (selectedOrderDetail != null)
            {
                try
                {
                    _storeContext.OrderDetails.Remove(selectedOrderDetail);
                    _storeContext.SaveChanges();
                    UpdateOrderDetailList();
                    HandleGridView();// Cập nhật danh sách chi tiết đơn hàng sau khi xóa
                    MessageBox.Show("Delete order detail successfully!", "Delete order detail");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to delete order detail!", "Delete order detail");
                }
            }
            else
            {
                MessageBox.Show("Please select an order detail to delete!", "Delete order detail");
            }
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                var orderDetails = _storeContext.OrderDetails.Where(o => o.OrderDetailId.ToString().Contains(searchText) ||
                                                                        o.Product.ProductName.Contains(searchText))
                                                             .ToList();
                if (orderDetails.Any())
                {
                    lvOrderDetail.ItemsSource = orderDetails;
                }
                else
                {
                    MessageBox.Show("No order details found!", "Search");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while searching for order details!", "Search");
            }
        }
        //ToolBar
        private void btnOpenOrdersManage_Click(object sender, RoutedEventArgs e)
        {
            var windowOrderManage = new WindowOrderManage(_storeContext);
            this.Close();
            windowOrderManage.Show();
            e.Handled = true;
        }

        private void btnOpenStaffManage_Click(object sender, RoutedEventArgs e)
        {
            var windowStaffManage = new WindowStaffManage(_storeContext);
            this.Close();
            windowStaffManage.Show();
            e.Handled = true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionService.Instance.ClearSession();
            var mainWindow = new MainWindow(_storeContext);
            this.Close();
            mainWindow.Show();
            e.Handled = true;
        }

        private void btnOpenProductsManage_Click(object sender, RoutedEventArgs e)
        {
            var windowProductManage = new WindowProductManage(_storeContext);
            this.Close();
            windowProductManage.Show();
            e.Handled = true;
        }
        private void btnOpenCategoriesManage_Click(object sender, RoutedEventArgs e)
        {
            var windowCategoryManage = new WindowCategoryManage(_storeContext);
            this.Close();
            windowCategoryManage.Show();
            e.Handled = true;
        }
        private void btnOpenMyAccount_Click(object sender, RoutedEventArgs e)
        {
            var WindowMyProfile = new WindowMyProfile(_storeContext);
            this.Close();
            WindowMyProfile.Show();
            e.Handled = true;
        }


        private void Selected_Product(object sender, RoutedEventArgs e)
        {

            var selected_product = (Product)cbbProductName.SelectedItem;
            txtUnitPrice.Text = selected_product.UnitPrice.ToString();

        }


        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrderDetailList();
        }

        private void btnOpenReport_Click(object sender, RoutedEventArgs e)
        {
            var windowReport = new WindowReport(_storeContext);
            this.Close();
            windowReport.Show();
            e.Handled = true;
        }
    }
}
