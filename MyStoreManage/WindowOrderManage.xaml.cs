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
    /// Interaction logic for WindowOrderManage.xaml
    /// </summary>
    public partial class WindowOrderManage : Window
    {
        private readonly MyStoreContext _storeContext;
        Order _selectedOrder;

        public WindowOrderManage(MyStoreContext storeContext)
        {
            InitializeComponent();
            _storeContext = storeContext;
            HandleStaffNameNavigate();
            ShowAllOrders();
            txtOrderDate.SelectedDate = DateTime.Now;
            HandleButttonRole();
        }
        public void HandleStaffNameNavigate()
        {
            txblStaffNameNavigate.Text = SessionService.Instance.GetNameInSession();
            txtStaffId.Text = SessionService.Instance.GetStaffIdInSession().ToString();
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
        public Order getOrderObject()
        {
            try
            {
                int staffId;
                if (int.TryParse(txtStaffId.Text, out staffId))

                    return new Order
                    {
                        OrderId = string.IsNullOrEmpty(txtOrderID.Text) ? 0 : Int32.Parse(txtOrderID.Text),
                        OrderDate = DateTime.Now,
                        StaffId = staffId
                    };
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot get product!");
            }
            return null;
        }
        private void ShowAllOrders()
        {
            List<Order> orders = GetAllOrders();
            if (orders != null)
            {
                lvOrder.ItemsSource = orders; // Gán danh sách đơn hàng vào ItemsSource của ListView
            }
            else
            {
                lvOrder.ItemsSource = null; // Đặt ItemsSource thành null nếu không có đơn hàng nào
            }
        }
        private List<Order> GetAllOrders()
        {
            try
            {
                return _storeContext.Orders.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy danh sách đơn hàng từ cơ sở dữ liệu!");
                return new List<Order>(); // Trả về danh sách rỗng nếu có lỗi
            }
        }
        private void updateGridView()
        {

            lvOrder.ItemsSource = GetAllOrders();
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            var orders = getOrderObject();
            try
            {

                if (orders != null)
                {
                    orders.OrderId = 0;
                    _storeContext.Orders.Add(orders);
                    _storeContext.SaveChanges();
                    updateGridView();
                    MessageBox.Show("add order success!", "add order");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot insert order!", "Add order");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = lvOrder.SelectedItem as Order;
            if (selectedOrder != null)
            {
                try
                {
                    // Cập nhật thông tin của đơn hàng từ các điều khiển nhập liệu
                    selectedOrder.OrderDate = txtOrderDate.SelectedDate ?? selectedOrder.OrderDate;
                    int staffId;
                    if (int.TryParse(txtStaffId.Text, out staffId))
                    {
                        selectedOrder.StaffId = staffId;
                    }

                    _storeContext.Entry<Order>(selectedOrder).State = EntityState.Modified;
                    _storeContext.SaveChanges();
                    updateGridView();
                    MessageBox.Show("Update order successfully!", "Update order");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to update order!", "Update order");
                }
            }
            else
            {
                MessageBox.Show("Please select an order to update!", "Update order");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = lvOrder.SelectedItem as Order;
            if (selectedOrder != null)
            {
                try
                {
                    _storeContext.Orders.Remove(selectedOrder);
                    _storeContext.SaveChanges();
                    updateGridView();
                    MessageBox.Show("Delete order successfully!", "Delete order");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to delete order!", "Delete order");
                }
            }
            else
            {
                MessageBox.Show("Please select an order to delete!", "Delete order");
            }

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int orderId;
                if (int.TryParse(txtSearch.Text, out orderId))
                {
                    var orders = _storeContext.Orders.Where(o => o.OrderId == orderId).ToList();
                    if (orders.Any())
                    {
                        lvOrder.ItemsSource = orders;
                    }
                    else
                    {
                        MessageBox.Show("No orders found!", "Search");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid Order ID!", "Search");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while searching for orders!", "Search");
            }
        }
        private void lvOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = lvOrder.SelectedItem as Order;

            if (selectedOrder != null)
            {
                _selectedOrder = selectedOrder;
                // Hiển thị thông tin của đơn hàng đã chọn trên các điều khiển nhập liệu
                txtOrderID.Text = selectedOrder.OrderId.ToString();
                txtOrderDate.SelectedDate = selectedOrder.OrderDate;
                txtStaffId.Text = selectedOrder.StaffId.ToString();
            }
        }
        private void lvOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedOrder = lvOrder.SelectedItem as Order;
            if (selectedOrder != null)
            {
                // Chuyển sang trang OrderDetail và truyền thông tin order
                var orderDetailPage = new WindowOrderDetailManage(_storeContext, selectedOrder);
                this.Close();
                orderDetailPage.Show();
                e.Handled = true;
            }
        }
        //ToolBar
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

        private void btnOrderdetai_Click(object sender, RoutedEventArgs e)
        {
            var WindowOrderdetailManage = new WindowOrderDetailManage(_storeContext, _selectedOrder);
            this.Close();
            WindowOrderdetailManage.Show();
            e.Handled = true;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            updateGridView();
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