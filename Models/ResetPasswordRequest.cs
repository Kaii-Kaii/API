﻿namespace QL_ThuChi.Models
{
    public class ResetPasswordRequest
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
        public int Otp { get; set; }
    }
}
