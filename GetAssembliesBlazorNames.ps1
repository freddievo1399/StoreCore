# 1. Lấy đường dẫn file cùng cấp (ví dụ file tên là data.ext)
$filePath = Join-Path -Path $PSScriptRoot -ChildPath "StoreCore.slnx"

# 2. Kiểm tra và đọc file
if (Test-Path $filePath) {
    [xml]$xmlData = Get-Content -Path $filePath -Raw -Encoding UTF8
    
    # 3. Tìm các node 'xxx' và in ra
    $nodes = $xmlData.SelectNodes("//Project")
   $filteredNodes = $nodes | Where-Object { $_.Path -match "\.Blazor" }

   # ... (Đoạn đọc file và lọc $filteredNodes của bạn giữ nguyên)

if ($filteredNodes) {
    # 1. Lấy danh sách tên sạch trước
    $cleanNames = $filteredNodes | ForEach-Object {
        $rawName = $_.Path.Split("/")[-1]
        $rawName -replace "\.csproj$", ""
    }

    # 2. Sắp xếp mảng dựa trên ký tự cuối cùng của mỗi chuỗi
    # $_[-1] nghĩa là lấy ký tự cuối cùng
    $sortedNames = $cleanNames | Sort-Object { $_[-1] }

    # 3. Định dạng lại với dấu ngoặc kép để chuẩn bị copy
    $results = $sortedNames | ForEach-Object { "$_" }

    # 4. Nối chuỗi xuống hàng và copy
    $finalContent = ($results -join "`r`n")
    $finalContent | Set-Clipboard

    Write-Host "--- Đã sắp xếp theo ký tự cuối và copy ---" -ForegroundColor Green
    Write-Host $finalContent
} else {
        Write-Host "Không tìm thấy node nào chứa chuỗi yêu cầu." -ForegroundColor Yellow
    }


} 
else {
    Write-Host "Lỗi: Không tìm thấy file 'data.ext' ở cùng thư mục với script!" -ForegroundColor Red
}

# 4. Dừng màn hình để xem kết quả
Write-Host "Nhấn phím bất kỳ để đóng..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")