using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreManage.session_login
{
    public class SessionService
    {
        private static SessionService _instance;
        public static SessionService Instance => _instance ?? (_instance = new SessionService());

        // Thông tin phiên
        private int _staffId;
        private int _role;
        private string _name;

        // Phương thức để đặt thông tin phiên khi đăng nhập
        public void SetSession(int staffId, int role, string name)
        {
            _staffId = staffId;
            _role = role;
            _name = name;
        }

        // Phương thức để lấy thông tin ID của người dùng từ phiên
        public int GetStaffIdInSession()
        {
            return _staffId;
        }

        // Phương thức để lấy thông tin vai trò của người dùng từ phiên
        public int GetRoleInSession()
        {
            return _role;
        }
        public string GetNameInSession()
        {
            return _name;
        }

        // Phương thức để xóa thông tin phiên khi người dùng đăng xuất
        public void ClearSession()
        {
            _staffId = 0;
            _role = 0;
            _name = null;
        }
    }

}
