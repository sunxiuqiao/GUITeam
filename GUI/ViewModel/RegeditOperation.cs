using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using GUI.View;
using System.Data;

namespace GUI.ViewModel
{
    public interface RegeditOperation
    {
        void writeRegedit(string ProjectName,string ConnName, string Server, string ServiceName, string DataBaseName, string PortNumber, string User, string PassWord);
    }

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Assert, Unrestricted = true)]
    public class GeoRegedit : RegeditOperation
    {
        public void writeRegedit(string ProjectName, string ConnName, string Server, string ServiceName, string DataBaseName, string PortNumber, string User, string PassWord)
        {
            RegistryKey software = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey aimdir = software.CreateSubKey("农村承包土地管理信息系统");
            RegistryKey temp = aimdir.CreateSubKey(ProjectName);
            RegistryKey GeoNameKey = temp.CreateSubKey("GeoRegistry");
            RegistryKey connNameKey = GeoNameKey.CreateSubKey(ConnName);
            connNameKey.SetValue("Server", Server);
            connNameKey.SetValue("ServiceName", ServiceName);
            connNameKey.SetValue("DataBaseName", DataBaseName);
            connNameKey.SetValue("PortNumber", PortNumber);
            connNameKey.SetValue("User", User);
            connNameKey.SetValue("PassWord", PassWord);
        }
    }

    public class AttrRegedit : RegeditOperation
    {

        public void writeRegedit(string ProjectName,string ConnName, string Server, string ServiceName, string DataBaseName, string PortNumber, string User, string PassWord)
        {
            RegistryKey software = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey aimdir = software.CreateSubKey("农村承包土地管理信息系统");
            RegistryKey temp = aimdir.CreateSubKey(ProjectName);
            RegistryKey AttriNameKey = temp.CreateSubKey("AttriRegistry");
            RegistryKey connNameKey = AttriNameKey.CreateSubKey(ConnName);
            connNameKey.SetValue("Server", Server);
            connNameKey.SetValue("ServiceName", ServiceName);
            connNameKey.SetValue("DataBaseName", DataBaseName);
            connNameKey.SetValue("PortNumber", PortNumber);
            connNameKey.SetValue("User", User);
            connNameKey.SetValue("PassWord", PassWord);
        }
    }
    public class BusRegedit : RegeditOperation
    {
        public void writeRegedit(string ProjectName, string ConnName, string Server, string ServiceName, string DataBaseName, string PortNumber, string User, string PassWord)
        {
            RegistryKey software = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey aimdir = software.CreateSubKey("农村承包土地管理信息系统");
            RegistryKey temp = aimdir.CreateSubKey(ProjectName);
            RegistryKey tempName = aimdir.CreateSubKey("BusiRegistry");
            RegistryKey connNameKey = tempName.CreateSubKey(ConnName);
            connNameKey.SetValue("Server", Server);
            connNameKey.SetValue("ServiceName", ServiceName);
            connNameKey.SetValue("DataBaseName", DataBaseName);
            connNameKey.SetValue("PortNumber", PortNumber);
            connNameKey.SetValue("User", User);
            connNameKey.SetValue("PassWord", PassWord);
        }
    }

    
}