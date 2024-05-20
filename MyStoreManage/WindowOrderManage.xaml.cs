using Microsoft.EntityFrameworkCore;
using MyStoreManage.Models;
using MyStoreManage.session_login;
using System.Windows;

namespace MyStoreManage
{
    /// <summary>
    /// Interaction logic for WindowOrderManage.xaml
    /// </summary>
    public partial class WindowOrderManage : Window
    {
        private readonly MyStoreContext _storeContext;
        public WindowOrderManage(MyStoreContext storeContext)
        {
            InitializeComponent();
            _storeContext = storeContext;
            HandleGirdView();
            HandleStaffNameNavigate();

            dpEndDate.SelectedDate = DateTime.Now.Date;
            dpStartDate.SelectedDate = DateTime.Now.Date.AddMonths(-1);
            HandleButtonAdmin();
        }
        public void HandleStaffNameNavigate()
        {
            txblStaffNameNavigate.Text = SessionService.Instance.GetNameInSession();
        }

        //public void HandleAdminGirdView()
        //{
        //    lvOrders.ItemsSource = _storeContext.Orders.Include(x => x.Staff).ToList();
        //}

        //public void HandleStaffGirdView()
        //{
        //    var idStaff = SessionService.Instance.GetStaffIdInSession();
        //    lvOrders.ItemsSource = _storeContext.Orders.Include(x => x.Staff).Where(od => od.StaffId == idStaff).ToList();
        //}

        public void HandleGirdView()
        {
            var role = SessionService.Instance.GetRoleInSession();
            if (role == 1)
            {
                lvOrders.ItemsSource = _storeContext.Orders.Include(x => x.Staff).ToList();

                //list order trong vong 1 thang ke tu ngay hien tai
                //DateTime currentDate = DateTime.Now;
                //DateTime oneMonthAgo = currentDate.AddMonths(-1);
                //lvOrders.ItemsSource = _storeContext.Orders
                //                        .Include(x => x.Staff)
                //                        .Where(od => od.OrderDate >= oneMonthAgo && od.OrderDate <= currentDate)
                //                        .ToList();

            }
            else if (role == 2)
            {
                var idStaff = SessionService.Instance.GetStaffIdInSession();
                lvOrders.ItemsSource = _storeContext.Orders.Include(x => x.Staff).Where(od => od.StaffId == idStaff).ToList();
            }

        }
        public void HandleButtonAdmin()
        {
            var role = SessionService.Instance.GetRoleInSession();
            if (role == 1)
            {
                //
            }
            else if (role == 2)
            {
                lbsearchID.Visibility = Visibility.Hidden;
                txtSearch.Visibility = Visibility.Hidden;
                btnSearch.Visibility = Visibility.Hidden;
            }

        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            if (int.TryParse(searchQuery, out int searchId))
            {
                // Tìm kiếm theo id
                var filteredOrders = _storeContext.Orders
                    .Include(x => x.Staff)
                    .Where(od => od.StaffId == searchId)
                    .ToList();

                lvOrders.ItemsSource = filteredOrders;
            }
            else
            {
                // Tìm kiếm theo name
                var filteredOrders = _storeContext.Orders
                    .Include(x => x.Staff)
                    .Where(od => od.Staff.Name.Contains(searchQuery))
                    .ToList();

                lvOrders.ItemsSource = filteredOrders;
            }
        }

        private void btnSearchDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime endDate = dpEndDate.SelectedDate ?? DateTime.Now.Date;
            DateTime startDate = dpStartDate.SelectedDate ?? endDate.AddMonths(-1);

            var filteredOrders = _storeContext.Orders
                .Include(x => x.Staff)
                .Where(od => od.OrderDate >= startDate.Date && od.OrderDate <= endDate.Date)
                .ToList();

            lvOrders.ItemsSource = filteredOrders;
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

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {

            if (int.TryParse(txtOrderID.Text, out int idDetail))
            {
                var windowOrderDetails = new WindowOrderDetailManage(idDetail, _storeContext);
                this.Close();
                windowOrderDetails.Show();
                e.Handled = true;
            }
            else
            {
                // Xử lý lỗi khi không thể chuyển đổi giá trị idDetail sang kiểu int
            }
        }
    }
}
