using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Shared.Constant
{
    public static class Messages
    {
        public static string UserRegistered = "Kullanıcı Kayıt Oldu.";
        public static string UserNotFound = "Kullanıcı Bulunamadı.";
        public static string PasswordError = "Şifre Hatalı";
        public static string SuccessfulLogin = "Giriş Başarılı";
        public static string UserAlreadyExists = "Kullanıcı Kullanılıyor.";
        public static string AccessTokenCreated = "Token Oluşturuldu.";
        public static string AccessWarning = "Yetki Bulunmamaktadır.";

        public static string SuccessProccess = "İşlem Başarılı.";
        public static string FailedProccess = "İşlem Başarısız.";
        public static string FailedCustomerProccess = "Bu bilgilerle kayıtlı bir kullanıcı zaten mevcut.";
        public static string RoleNotFound = "Rol bulunamadı.";
        public static string SurveySuccessCompleted =  "Anket başarıyla oluşturuldu.";
    }
}
