///////////////////////// Code /////////////////////////
int soNgay;
double chiPhiThuoc;
int canNang;

OnThiWinformApp1Entities db = new OnThiWinformApp1Entities();


private void Form1_FormClosing(object sender, FormClosingEventArgs e)
{
    if (MessageBox.Show("Bạn có chắc muốn thoát ứng dụng không?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
    e.Cancel = true;
}

private void Form1_Load(object sender, EventArgs e)
{
    resetListView(db.ChamSocThuCungs.ToList());
}

private void rbtnChuaBenh_CheckedChanged(object sender, EventArgs e)
{
    lblChiPhiThuoc.Visible = true;
    txtChiPhiThuoc.Visible = true;
    txtChiPhiThuoc.Clear();
    lblSoNgay.Visible = false;
    txtSoNgay.Visible = false;
}

private void rbtnChamSocHo_CheckedChanged(object sender, EventArgs e)
{
    lblChiPhiThuoc.Visible = false;
    txtChiPhiThuoc.Visible = false;
    txtSoNgay.Clear();
    lblSoNgay.Visible = true;
    txtSoNgay.Visible = true;
}

public void reset()
{
    txtMaDon.Clear();
    txtTenThuCung.Clear();
    txtChungLoai.Clear();
    txtCanNang.Clear();
    dtpNgayNhan.Value = DateTime.Now;
    txtTinhTrang.Clear();
    rbtnChuaBenh.Checked = true;
    rbtnChamSocHo.Checked = false;
    txtChiPhiThuoc.Clear();
    txtSoNgay.Clear();

    txtMaDon.Focus();
}

public void resetListView(IEnumerable<ChamSocThuCung> cstc)
{
    lvDanhSachThuCung.Items.Clear();

    foreach (var item in cstc)
    {
        ListViewItem data = new ListViewItem(item.MaDon);

        if(item.CanNang > 40)
        {
            data.BackColor = Color.Yellow;
        }

        data.SubItems.Add(item.TenThuCung);
        data.SubItems.Add(item.ChungLoai);
        data.SubItems.Add(item.NgayNhan.ToString());

        lvDanhSachThuCung.Items.Add(data);
    }
}

private void btnThem_Click(object sender, EventArgs e)
{
    reset();
}

public bool kiemTra()
{
    if (string.IsNullOrEmpty(txtMaDon.Text)
        || string.IsNullOrEmpty(txtTenThuCung.Text)
        || string.IsNullOrEmpty(txtChungLoai.Text)
        || string.IsNullOrEmpty(txtCanNang.Text)
        || string.IsNullOrEmpty(txtTinhTrang.Text)
        || (string.IsNullOrEmpty(txtSoNgay.Text) && rbtnChamSocHo.Checked)
        || (string.IsNullOrEmpty(txtChiPhiThuoc.Text) && rbtnChuaBenh.Checked))
    {
        MessageBox.Show("Thông tin không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    if (dtpNgayNhan.Value > DateTime.Now)
    {
        MessageBox.Show("Ngày nhận không được lớn hơn ngày hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    if ((!int.TryParse(txtSoNgay.Text, out soNgay) || soNgay <= 0)
        && rbtnChamSocHo.Checked)
    {
        MessageBox.Show("Số ngày không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    if ((!double.TryParse(txtChiPhiThuoc.Text, out chiPhiThuoc) || chiPhiThuoc <= 0) && rbtnChuaBenh.Checked)
    {
        MessageBox.Show("Chi phí thuốc không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    if (!int.TryParse(txtCanNang.Text, out canNang) || canNang <= 0)
    {
        MessageBox.Show("Cân nặng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    return true;
}

private void btnLuu_Click(object sender, EventArgs e)
{

    if (kiemTra())
    {
        double phiDichVu = 0;

        if (rbtnChamSocHo.Checked)
            phiDichVu = 200000 * (soNgay);
        else if (rbtnChuaBenh.Checked)
            phiDichVu = 100000 + chiPhiThuoc;

        bool check = db.ChamSocThuCungs.Any(x => x.MaDon.Equals(txtMaDon.Text));

        if (!check)
        {
            db.ChamSocThuCungs.Add(new ChamSocThuCung()
            {
                MaDon = txtMaDon.Text,
                CanNang = canNang,
                ChungLoai = txtChungLoai.Text,
                NgayNhan = dtpNgayNhan.Value,
                PhiDichVu = phiDichVu,
                TenThuCung = txtTenThuCung.Text,
                TinhTrang = txtTinhTrang.Text,
                DichVu = rbtnChuaBenh.Checked ? true : false,

            });

            db.SaveChanges();

            resetListView(db.ChamSocThuCungs.ToList());
        }
        else
        {
            MessageBox.Show("Mã đơn đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }
}

private void lvDanhSachThuCung_SelectedIndexChanged(object sender, EventArgs e)
{
    if (lvDanhSachThuCung.SelectedItems.Count > 0)
    {
        txtMaDon.Enabled = false;

        string maDon = lvDanhSachThuCung.SelectedItems[0].SubItems[0].Text;

        ChamSocThuCung c = db.ChamSocThuCungs.Find(maDon);
                
        if (c != null)
        {
            txtMaDon.Text = c.MaDon;
            txtTenThuCung.Text = c.TenThuCung;
            txtCanNang.Text = c.CanNang.ToString();
            txtChungLoai.Text = c.ChungLoai;
            dtpNgayNhan.Value = c.NgayNhan;
            txtTinhTrang.Text = c.TinhTrang;

            if (c.DichVu == true)
            {
                rbtnChuaBenh.Checked = true;
                txtChiPhiThuoc.Text = (c.PhiDichVu - 100000).ToString();
            }
            else
            {
                rbtnChamSocHo.Checked = true;
                txtSoNgay.Text = (c.PhiDichVu/200000).ToString();
            }

        }
        else
        {
            MessageBox.Show("Không tìm thấy thú cưng!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
            return;
        }
    }
    else
    {
        reset();
        txtMaDon.Enabled = true;
    }
}

private void btnXoa_Click(object sender, EventArgs e)
{
    if (lvDanhSachThuCung.SelectedItems.Count > 0)
    {

        string maDon = lvDanhSachThuCung.SelectedItems[0].SubItems[0].Text;

        ChamSocThuCung c = db.ChamSocThuCungs.Find(maDon);

        if (c != null)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.ChamSocThuCungs.Remove(c);
                db.SaveChanges();

                resetListView(db.ChamSocThuCungs.ToList());
            }
        }
        else
        {
            MessageBox.Show("Không tìm thấy thú cưng!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
            return;
        }
    }
    else
    {
        MessageBox.Show("Hãy chọn thú cưng muốn xóa!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
        return;
    }
}

private void btnSua_Click(object sender, EventArgs e)
{
    if (lvDanhSachThuCung.SelectedItems.Count > 0)
    {
        string maDon = lvDanhSachThuCung.SelectedItems[0].SubItems[0].Text;
        ChamSocThuCung c = db.ChamSocThuCungs.Find(maDon);

        if (c != null)
        {
            if (kiemTra())
            {
                double phiDichVu = 0;

                if (rbtnChamSocHo.Checked)
                    phiDichVu = 200000 * (soNgay);
                else if (rbtnChuaBenh.Checked)
                    phiDichVu = 100000 + chiPhiThuoc;

                // Cập nhật thuộc tính của đối tượng ChamSocThuCung
                c.CanNang = canNang;
                c.ChungLoai = txtChungLoai.Text;
                c.NgayNhan = dtpNgayNhan.Value;
                c.PhiDichVu = phiDichVu;
                c.TenThuCung = txtTenThuCung.Text;
                c.TinhTrang = txtTinhTrang.Text;
                c.DichVu = rbtnChuaBenh.Checked;

                db.SaveChanges();

                resetListView(db.ChamSocThuCungs.ToList());

            };
        }
        else
        {
            MessageBox.Show("Không tìm thấy thú cưng!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
            return;
        }
    }
    else
    {
        MessageBox.Show("Hãy chọn thú cưng muốn sửa!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
        return;
    }
}

private void btnSapXep_Click(object sender, EventArgs e)
{
    var s = db.ChamSocThuCungs.OrderByDescending(p => p.NgayNhan).ThenBy(p => p.CanNang).ToList();

    resetListView(s);
}

private void btnThongKe_Click(object sender, EventArgs e)
{
    List<ChamSocThuCung> danhSachDon = db.ChamSocThuCungs.ToList();
           
    var a = danhSachDon
        .GroupBy(c => c.DichVu)
        .Select(group => new
        {
            dichVu = group.Key,
            slDon = group.Count(),
            doanhThu = group.Sum(c => c.PhiDichVu)
        });

    string message = "";

    foreach (var i in a)
    {
        if (i.dichVu == true)
            message += $"Loại dịch vụ: Chữa bệnh\n";
        else
            message += $"Loại dịch vụ: Chăm sóc hộ\n";

        message += $"Tổng số lượng đơn: {i.slDon}\n";
        message += $"Tổng doanh thu: {i.doanhThu}\n";
    }

    MessageBox.Show(message, "Thống kê", MessageBoxButtons.OK, MessageBoxIcon.Information);
}


/////////////////////// Khác ///////////////////////

private void Reset()
{
    foreach (ListViewItem item in lv_DSPhongVien.Items)
    {
        item.Selected = false;
    }
    txt_MaNV.Clear();
    txt_HoTen.Clear();
    txt_DienThoai.Clear();
    dtp_NgayVaoLam.Value = DateTime.Now;
    rad_Nam.Checked = true;
    rad_TaiToa.Checked = true;
    txt_SoGioLam.Clear();
    txt_PhuCap.Clear();
}

private void btn_Luu_Click(object sender, EventArgs e)
{
    if (KiemTra())
    {
        float pt = 12000000;
        bool c = db.PhongVien.Any(d => d.MaNV == txt_MaNV.Text);
        if (rad_TaiToa.Checked)
            pt = pt + (float.Parse(txt_SoGioLam.Text) * 100000 * (float)1.5);
        else
            pt = pt + float.Parse(txt_PhuCap.Text);
        int y = 0;
        if (c)
            MessageBox.Show("Mã phóng viên đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        else
        {
            db.PhongVien.Add(new PhongVien()
            {
                MaNV = txt_MaNV.Text,
                HoTen = txt_HoTen.Text,
                SoDienThoai = txt_DienThoai.Text,
                GioiTinh = rad_Nam.Checked ? true : false,
                NgayVaoLam = dtp_NgayVaoLam.Value,
                LoaiPV = rad_TaiToa.Checked ? true : false,
                Luong = pt
            });
            db.SaveChanges();

            ListViewItem i = new ListViewItem(txt_MaNV.Text);
            i.SubItems.Add(txt_HoTen.Text);
            i.SubItems.Add(rad_Nam.Checked ? rad_Nam.Text : rad_Nu.Text);
            i.SubItems.Add(dtp_NgayVaoLam.Value.ToString("dd/MM/yyyy"));

            y = DateTime.Now.Year - dtp_NgayVaoLam.Value.Year;

            lv_DSPhongVien.Items.Add(i);

            i.Selected = true;
            if (y >= 5)
            {
                i.BackColor = Color.Yellow;
            }
        }
    }
}

private void btn_Xoa_Click(object sender, EventArgs e)
{
    if (lv_DSPhongVien.SelectedItems.Count > 0)
    {
        if (MessageBox.Show("Bạn có chắc chắn muốn xóa phóng viên đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            int index = lv_DSPhongVien.Items.IndexOf(lv_DSPhongVien.SelectedItems[0]);

            string maDon = lv_DSPhongVien.SelectedItems[0].SubItems[0].Text;

            PhongVien pv = db.PhongVien.Where(p => p.MaNV.Trim() == maDon).SingleOrDefault();
            db.PhongVien.Remove(pv);
            db.SaveChanges();

            lv_DSPhongVien.Items.Remove(lv_DSPhongVien.SelectedItems[0]);

            if (lv_DSPhongVien.Items.Count > 0)
            {
                if (index < lv_DSPhongVien.Items.Count)
                    lv_DSPhongVien.Items[index].Selected = true;
                else
                    Reset();
            }

            if (lv_DSPhongVien.Items.Count == 0)
            {
                Reset();
            }
        }
    }
    else
    {
        MessageBox.Show("Vui lòng chọn một phóng viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}

private void btn_Sua_Click(object sender, EventArgs e)
{
    if (lv_DSPhongVien.SelectedItems.Count > 0)
    {
        int index = lv_DSPhongVien.Items.IndexOf(lv_DSPhongVien.SelectedItems[0]);

        string maNV = lv_DSPhongVien.SelectedItems[0].SubItems[0].Text;
        PhongVien pv = db.PhongVien.Where(p => p.MaNV.Trim() == maNV).SingleOrDefault();

        float pt = 12000000;
        if (rad_TaiToa.Checked)
            pt = pt + (float.Parse(txt_SoGioLam.Text) * 100000 * (float)1.5);
        else
            pt = pt + float.Parse(txt_PhuCap.Text);

        if (KiemTra())
        {
            pv.HoTen = txt_HoTen.Text;
            pv.SoDienThoai = txt_DienThoai.Text;
            pv.GioiTinh = rad_Nam.Checked ? true : false;
            pv.NgayVaoLam = dtp_NgayVaoLam.Value;
            pv.LoaiPV = rad_TaiToa.Checked ? true : false;
            pv.Luong = pt;
            db.SaveChanges();
            txt_MaNV.Enabled = true;
            ResetListView(db.PhongVien.ToList());
            Reset();
        }
    }
    else
    {
        MessageBox.Show("Vui lòng chọn một phóng viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}

private void btn_SapXep_Click(object sender, EventArgs e)
{
    try
    {
        var s = db.PhongVien.OrderBy(p => p.NgayVaoLam).ThenBy(p => p.HoTen).ToList();
        ResetListView(s);
    }
    catch (Exception ex)
    {
        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void btn_ThongKe_Click(object sender, EventArgs e)
{
    try
    {
        var s = from pv in db.PhongVien
                group pv by pv.LoaiPV into g
                select new
                {
                    LoaiPV = g.Key,
                    TongSoLuong = g.Count(),
                    TongLuong = g.Sum(p => p.Luong)
                };

        string message = "Thống kê theo loại phóng viên\n\n";

        foreach (var t in s)
        {
            if (t.LoaiPV)
                message += $"Phóng viên tại tòa soạn:\n";
            else
                message += $"Phóng viên thường trú:\n";
            message += $"Số lượng: {t.TongSoLuong}\n";
            message += $"Tổng lương chi: {t.TongLuong:#,#}\n\n";
        }

        MessageBox.Show(message, "Thống kê", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

/////////////////////// Xuất excel, In báo cáo ///////////////////////
using Excel = Microsoft.Office.Interop.Excel;
using Form_Rpt = CrystalDecisions.CrystalReports.Engine;

private void btnXuatExcel_Click(object sender, EventArgs e)
{
    Excel.Application excelApp = new Excel.Application();
    Excel.Workbook excelWb = excelApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
    Excel.Worksheet excelWs = excelWb.Worksheets[1];

    Excel.Range excelRange = excelWs.Cells[1, 1];
    excelRange.Font.Size = 16;
    excelRange.Font.Bold = true;
    excelRange.Font.Color = Color.Blue;
    excelRange.Value = "DANH MỤC SẢN PHẨM";

    // Lấy danh mục
    var catalogs = db.Catalogs.Select(c => new { Code = c.CatalogCode, Name = c.CatalogName }).ToList();
    int row = 2;
    foreach (var c in catalogs)
    {
        excelWs.Range["A" + row].Font.Bold = true; excelWs.Range["A" + row].Value = c.Name;
        row++;
        // Lấy sp theo danh mục
        var products = from p in db.Products where p.CatalogCode == c.Code select p;
        foreach (var p in products)
        {
            excelWs.Range["A" + row].Value = p.ProductCode;
            excelWs.Range["B" + row].ColumnWidth = 50;
            excelWs.Range["B" + row].Value = p.ProductName;
            excelWs.Range["C" + row].Value = p.ProductPrice;
            row++;
        }
    }

    excelWs.Name = "DanhMucSanPham"; excelWb.Activate();
    // Luu file
    SaveFileDialog saveFileDialog = new SaveFileDialog(); if (saveFileDialog.ShowDialog() == DialogResult.OK) excelWb.SaveAs(saveFileDialog.FileName);
    excelApp.Quit();
}

private void btnInBaoCao_Click(object sender, EventArgs e)
{
    // Chuẩn bị nguồn dữ liệu
    var data = db.Products.Select(p => new {
        ProductCode = p.ProductCode,
        ProductName = p.ProductName,
        ProductPrice = p.ProductPrice,
        CatalogCode = p.Catalog.CatalogCode,
        CatalogName = p.Catalog.CatalogName
    }).ToList();

    // Gán nguồn dữ liệu cho CrystalReport
    CrystalReport1 rpt = new CrystalReport1();
    rpt.SetDataSource(data);

    // Hiển thị báo cáo
    Form2 fRpt = new Form2();
    fRpt.crystalReportViewer1.ReportSource = rpt;
    fRpt.ShowDialog();
}