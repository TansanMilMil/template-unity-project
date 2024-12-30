using System.Collections;
using System.Collections.Generic;

namespace TansanMilMil.Util
{
    public interface IKeyBind
    {
        public IEnumerable<InputKeyBind> GetKeyRoleTalk();
        public IEnumerable<InputKeyBind> GetKeyRoleDecide();
        public IEnumerable<InputKeyBind> GetKeyRoleCancel();
        public IEnumerable<InputKeyBind> GetKeyRolePause();
        public IEnumerable<InputKeyBind> GetKeyRoleOptionA();
        public IEnumerable<InputKeyBind> GetKeyRoleOptionB();
        public IEnumerable<InputKeyBind> GetKeyRoleSkip();
    }
}