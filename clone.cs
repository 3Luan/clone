namespace App1
{
    public partial class Form1 : Form
    {
        int soNgay;
        double chiPhiThuoc;
        int canNang;

        OnThiWinformApp1Entities db = new OnThiWinformApp1Entities();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát ứng dụng không?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            e.Cancel = true;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            resetListView(db.ChamSocThuCungs.ToList());
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
    }
}