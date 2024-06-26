﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;
using UserRLL.Entity;

namespace UserBLL.Interface
{
    public interface IUserBL
    {
        UserModel AddUser(UserModel model);
        UserEntity Login(LoginModel login);

        IEnumerable<UserEntity> GetUsers();
    }
}
