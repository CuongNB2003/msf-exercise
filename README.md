# Dự Án Cá Nhân Làm Tại Công Ty MSF

## Lưu Ý

Khi muốn kiểm tra log ở server, hãy sử dụng: `System.Diagnostics.Debug.WriteLine`.

### Ví Dụ Minh Họa

```csharp
var authHeader = Request.Headers["Authorization"].ToString();
System.Diagnostics.Debug.WriteLine($"Authorization Header: {authHeader}");

foreach (var claim in User.Claims)
{
    System.Diagnostics.Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
}

