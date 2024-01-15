x
x
x
x
x
xx
x
xx
x
x
xx
x
x
x

////////////////////////////////////////////

-	Tạo action lấy sản phẩm theo từng mã phân loại (phụ), code tự gõ
       public ActionResult GetProductsByCategory(string cateId) 
       { 
           var products = db.SanPhams.Where(p => p.MaPhanLoaiPhu == cateId).ToList();

           // tránh lỗi tham chiếu vòng tròn
           var _products = products.Select(p => new
           {
               MaSanPham = p.MaSanPham,
               TenSanPham = p.TenSanPham,
               MaPhanLoai = p.MaPhanLoai,
               GiaNhap = p.GiaNhap,
               DonGiaBanNhoNhat = p.DonGiaBanNhoNhat,
               DonGiaBanLonNhat = p.DonGiaBanLonNhat,
               TrangThai = p.TrangThai,
               MoTaNgan = p.MoTaNgan,
               AnhDaiDien = p.AnhDaiDien,
               NoiBat = p.NoiBat,
               MaPhanLoaiPhu = p.MaPhanLoaiPhu,
           }).ToList();

           return Json(new { products = _products }, JsonRequestBehavior.AllowGet);
       }

-	Thêm id cho thẻ html chứa các sản phẩm, tự gõ
 
-	Nhớ thêm type
 

-	Trong view Index, tạo hàm sự kiện sử dụng ajax để lấy danh sách sản phẩm theo mã phân loại phụ, tự gõ
@* phải có dòng này mới sài được ajax *@
@Scripts.Render("~/Content/template/vendor/jquery/jquery-3.2.1.min.js")

<script>

    // hàm sự kiện js khi chọn phân loại
    function chooseCategory(cateId) {
    $.ajax({
        url: '@Url.Action("GetProductsByCategory", "Home")',
        method: 'GET',
        dataType: 'json',
        data: { cateId: cateId },
        success: function (data) {

            /*
               mình phải lấy data.products vì trong action, danh sách sản phẩm trả về có tên: products
            */
           // truyền danh sách này tới hàm load sản phẩm
            loadSP(data.products)
        },
        error: function (err) {
            console.log(err)
        }
    })
}</script>

-	Lưu ý:
  
-	Tạo hàm js load sản phẩm lên view
Lưu ý: biến str = `` chứ không phải str = ''
// hàm load sản phẩm lên view
function loadSP(sanPhams) {

    // lấy thẻ html chứa các sản phẩm
    var spContainer = document.querySelector('#sp-container');

    // xóa hết sản phẩm trước đó
    spContainer.innerHTML = '';

    // str để lưu code html của các sản phẩm
    let str = ``;

    /*
        duyệt qua các sản phẩm, mỗi sản phẩm là 1 thẻ html, cộng hết vào biến str, nhớ đổi giá trị của sản phẩm
    */
    for (let p of sanPhams) {
        str += `<div class="col-sm-6 col-md-4 col-lg-3 p-b-35 isotope-item women">
                    <div class="block2">
                        <div class="block2-pic hov-img0">
                            <img src="/Content/Images QLQuanAo/${p.AnhDaiDien}" alt="IMG-PRODUCT">

                            <a href="#" class="block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 ">
                                Quick View
                            </a>
                        </div>

                        <div class="block2-txt flex-w flex-t p-t-14">
                            <div class="block2-txt-child1 flex-col-l ">
                                <a href="product-detail.html" class="stext-104 cl4 hov-cl1 trans-04 js-name-b2 p-b-6">
                                    ${p.TenSanPham}
                                </a>

                                <span class="stext-105 cl3">
                                    $${p.DonGiaBanNhoNhat}
                                </span>
                            </div>

                            <div class="block2-txt-child2 flex-r p-t-3">
                                <a href="#" class="btn-addwish-b2 dis-block pos-relative js-addwish-b2">
                                    <img class="icon-heart1 dis-block trans-04" src="/Content/template/images/icons/icon-heart-01.png" alt="ICON">
                                    <img class="icon-heart2 dis-block trans-04 ab-t-l" src="/Content/template/images/icons/icon-heart-02.png" alt="ICON">
                                </a>
                            </div>
                        </div>
                    </div>
                </div>`
    }

    // đưa str vào thẻ html chứa các sản phẩm
    $('#sp-container').html(str);
}



////////////////////////////////////

@RenderPage("Header.cshtml");

<div class="container body-content">
    @RenderBody()
</div>
@RenderPage("Footer.cshtml");

@Scripts.Render("~/bundles/jquery")
@Scripts.Render("~/bundles/bootstrap")
@RenderSection("scripts", required: false)

///////////////////////////////////////

<script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
<script>
    $(".grid_sorting_button").click(function () {
        // Lấy giá trị hiển thị của phần tử được click
        var phanLoaiValue = $(this).text().trim();
        console.log("click");

        $.ajax({
            url: '@Url.Action("GetProductByCategory", "Home")',
            dataType: "json",
            type: "GET",
            data: { phanloai: phanLoaiValue },
            success: function (data) {
                console.log("data", data);

                // lấy thẻ html chứa các sản phẩm
                $('#isotope-grid').empty();

                $.each(data.sanPham, function (key, val) {
                    var newRow = `
                    <div class="col-sm-6 col-md-4 col-lg-3 p-b-35 isotope-item women">
                        <div class="block2">
                            <div class="block2-pic hov-img0">
                                <img src="/Content/${val.AnhDaiDien}" alt="IMG-PRODUCT">
                                <a href="SuaSanPham/${val.MaSanPham}" class="block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1">
                                    Chi tiết sản phẩm
                                </a>
                            </div>
                            <div class="block2-txt flex-w flex-t p-t-14">
                                <div class="block2-txt-child1 flex-col-l ">
                                    <a href="product-detail.html" class="stext-104 cl4 hov-cl1 trans-04 js-name-b2 p-b-6">
                                        ${val.TenSanPham}
                                    </a>
                                    <span class="stext-105 cl3">
                                        ${val.DonGiaBanNhoNhat}
                                    </span>
                                </div>
                                <div class="block2-txt-child2 flex-r p-t-3">
                                    <a href="#" class="btn-addwish-b2 dis-block pos-relative js-addwish-b2">
                                        <img class="icon-heart1 dis-block trans-04" src="/Content/images/icons/icon-heart-01.png" alt="ICON">
                                        <img class="icon-heart2 dis-block trans-04 ab-t-l" src="/Content/images/icons/icon-heart-02.png" alt="ICON">
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>`;
                    $('#isotope-grid').append(newRow);
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log('AJAX Error:', textStatus, errorThrown);
                console.log('Status Code:', jqXHR.status);
                console.log('Response Text:', jqXHR.responseText);
            }
        });
    });
</script>

public ActionResult GetProductByCategory(string phanloai)
{
    if (phanloai == "Tất cả sản phẩm")
    {
        var sanPham1 = db.SanPhams.ToList();

        var _sanPham1 = sanPham1
        .Select(sp => new SanPham
        {
            MaSanPham = sp.MaSanPham,
            TenSanPham = sp.TenSanPham,
            DonGiaBanNhoNhat = sp.DonGiaBanNhoNhat,
            DonGiaBanLonNhat = sp.DonGiaBanLonNhat,
            TrangThai = sp.TrangThai,
            MoTaNgan = sp.MoTaNgan,
            AnhDaiDien = sp.AnhDaiDien,
            NoiBat = sp.NoiBat,
            MaPhanLoaiPhu = sp.MaPhanLoaiPhu,
            MaPhanLoai = sp.MaPhanLoai,
            GiaNhap = sp.GiaNhap
        }).ToList();
        return Json(new { sanPham = _sanPham1 }, JsonRequestBehavior.AllowGet);
    }
    var sanPham = db.SanPhams
        .Where(sp => sp.PhanLoaiPhu.TenPhanLoaiPhu == phanloai)
        .ToList();

    // tạo danh sách mới chỉ lấy các thuộc tính
    // không lấy các đối tượng tham chiếu
    var _sanPham = sanPham
        .Select(sp => new SanPham
        {
            MaSanPham = sp.MaSanPham,
            TenSanPham = sp.TenSanPham,
            DonGiaBanNhoNhat = sp.DonGiaBanNhoNhat,
            DonGiaBanLonNhat = sp.DonGiaBanLonNhat,
            TrangThai = sp.TrangThai,
            MoTaNgan = sp.MoTaNgan,
            AnhDaiDien = sp.AnhDaiDien,
            NoiBat = sp.NoiBat,
            MaPhanLoaiPhu = sp.MaPhanLoaiPhu,
            MaPhanLoai = sp.MaPhanLoai,
            GiaNhap = sp.GiaNhap
        }).ToList();

    return Json(new { sanPham = _sanPham }, JsonRequestBehavior.AllowGet);
}

//////////////////////////////////////////////////

viewbag:
var phanloai = db.PhanLoaiPhus.ToList();
var sanpham = db.SanPhams.ToList();
ViewBag.PhanLoai = phanloai;
return View(sanpham);

List<OnTap2.Models.PhanLoaiPhu> phanLoaiPhus = ViewBag.PhanLoai;
var i in Model
var i in phanLoaiPhus

///////////////////////////////////////

viewdata:
ViewData["PhanLoaiPhu"] = db.PhanLoaiPhus.ToList();
ViewData["SanPham"] = db.SanPhams.ToList();
return View();

foreach (var item in ViewData["PhanLoaiPhu"] as List<giaibai2.Models.PhanLoaiPhu>)
foreach (var item in ViewData["SanPham"] as List<giaide1.Models.SanPham> ?? new List<giaide1.Models.SanPham>())

///////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

[DisplayName("Tên Sản Phẩm")]
[Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
[RegularExpression(@"^\d+$", ErrorMessage = "Vui lòng nhập số")]
[RegularExpression(@".*\.(jpg)$", ErrorMessage = "Ảnh đại diện phải có định dạng 'jpg'")]
[RegularExpression("^[a-zA-Z].*", ErrorMessage = "Ký tự đầu tiên phải là chữ cái.")]



/////////////////////////////////

using giaibai2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace giaibai2.Controllers
{
    public class HomeController : Controller
    {
        QLBanHangQuanAoEntities db = new QLBanHangQuanAoEntities();
        public ActionResult Index()
        {
            ViewData["PhanLoaiPhu"] = db.PhanLoaiPhus.ToList();
            ViewData["SanPham"] = db.SanPhams.ToList();
            return View();
        }

        public ActionResult GetProductByCategory(string phanloai)
        {
            if (phanloai == "Tất cả sản phẩm")
            {
                var sanPham1 = db.SanPhams.ToList();

                var _sanPham1 = sanPham1
                .Select(sp => new SanPham
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    DonGiaBanNhoNhat = sp.DonGiaBanNhoNhat,
                    DonGiaBanLonNhat = sp.DonGiaBanLonNhat,
                    TrangThai = sp.TrangThai,
                    MoTaNgan = sp.MoTaNgan,
                    AnhDaiDien = sp.AnhDaiDien,
                    NoiBat = sp.NoiBat,
                    MaPhanLoaiPhu = sp.MaPhanLoaiPhu,
                    MaPhanLoai = sp.MaPhanLoai,
                    GiaNhap = sp.GiaNhap
                }).ToList();
                return Json(new { sanPham = _sanPham1 }, JsonRequestBehavior.AllowGet);
            }
            var sanPham = db.SanPhams
                .Where(sp => sp.PhanLoaiPhu.TenPhanLoaiPhu == phanloai)
                .ToList();

            // tạo danh sách mới chỉ lấy các thuộc tính
            // không lấy các đối tượng tham chiếu
            var _sanPham = sanPham
                .Select(sp => new SanPham
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    DonGiaBanNhoNhat = sp.DonGiaBanNhoNhat,
                    DonGiaBanLonNhat = sp.DonGiaBanLonNhat,
                    TrangThai = sp.TrangThai,
                    MoTaNgan = sp.MoTaNgan,
                    AnhDaiDien = sp.AnhDaiDien,
                    NoiBat = sp.NoiBat,
                    MaPhanLoaiPhu = sp.MaPhanLoaiPhu,
                    MaPhanLoai = sp.MaPhanLoai,
                    GiaNhap = sp.GiaNhap
                }).ToList();

            return Json(new { sanPham = _sanPham }, JsonRequestBehavior.AllowGet);
        }


        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaPhanLoai = new SelectList(db.PhanLoais, "MaPhanLoai", "PhanLoaiChinh");
            ViewBag.MaPhanLoaiPhu = new SelectList(db.PhanLoaiPhus, "MaPhanLoaiPhu", "TenPhanLoaiPhu");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSanPham,TenSanPham,MaPhanLoai,GiaNhap,DonGiaBanNhoNhat,DonGiaBanLonNhat,TrangThai,MoTaNgan,AnhDaiDien,NoiBat,MaPhanLoaiPhu")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaPhanLoai = new SelectList(db.PhanLoais, "MaPhanLoai", "PhanLoaiChinh", sanPham.MaPhanLoai);
            ViewBag.MaPhanLoaiPhu = new SelectList(db.PhanLoaiPhus, "MaPhanLoaiPhu", "TenPhanLoaiPhu", sanPham.MaPhanLoaiPhu);
            return View(sanPham);
        }

        public ActionResult SuaSanPham(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhanLoai = new SelectList(db.PhanLoais, "MaPhanLoai", "PhanLoaiChinh", sanPham.MaPhanLoai);
            ViewBag.MaPhanLoaiPhu = new SelectList(db.PhanLoaiPhus, "MaPhanLoaiPhu", "TenPhanLoaiPhu", sanPham.MaPhanLoaiPhu);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaSanPham([Bind(Include = "MaSanPham,TenSanPham,MaPhanLoai,GiaNhap,DonGiaBanNhoNhat,DonGiaBanLonNhat,TrangThai,MoTaNgan,AnhDaiDien,NoiBat,MaPhanLoaiPhu")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPhanLoai = new SelectList(db.PhanLoais, "MaPhanLoai", "PhanLoaiChinh", sanPham.MaPhanLoai);
            ViewBag.MaPhanLoaiPhu = new SelectList(db.PhanLoaiPhus, "MaPhanLoaiPhu", "TenPhanLoaiPhu", sanPham.MaPhanLoaiPhu);
            return View(sanPham);
        }


        // GET: SanPhams/Details/5
        public ActionResult ChiTietSanPham(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("ChiTietSanPham")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}




