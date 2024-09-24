Dự án cá nhân làm tại công ty msf


lưu ý khi muốn chek log ở server hãy sử dụng: System.Diagnostics.Debug.WriteLine
ví dụ minh họa
var authHeader = Request.Headers["Authorization"].ToString();
System.Diagnostics.Debug.WriteLine($"Authorization Header: {authHeader}");

foreach (var claim in User.Claims)
{
    System.Diagnostics.Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
}
