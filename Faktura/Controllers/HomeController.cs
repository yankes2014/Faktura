using Faktura.Context;
using Faktura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Faktura.Controllers
{
    public class HomeController : Controller
    {
        FakturaContext context = new FakturaContext();


        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// Strona główna na której wprowadzamy dane Firmy(nabywcy) i elementy
        /// Wyswietlamy na niej produkty z bazy danych
        /// </summary>
        /// <returns></returns>>
        public ActionResult Order()
        {
            var productsFromDataBase = context.Products.AsNoTracking().ToList();
            var products = new HashSet<Product>();
            foreach (var item in productsFromDataBase)
            {
                products.Add(item);
            }

            ViewData["Products"] = products;

            return View();
        }

        /// <summary>
        /// Metoda Post z formularza na stronie głównej Order
        /// Zapisuje w bazie dane z formularza i przechodzi do widoku faktury
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [HttpPost]
        public RedirectResult CreateInvoice(Firm firm)
        {

            //Dodanie firmy do bazy firm
            context.Firms.Add(firm);
            context.SaveChanges();

            //Tworzymy fakture
            Invoice invoice = new Invoice();

            //Dopisanie firmy do faktury
            invoice.Firm = context.Firms.Where(n => n.NIP == firm.NIP && n.Address == firm.Address && n.Mail == firm.Mail && n.OwnerName == firm.OwnerName).FirstOrDefault();
            invoice.FirmID = invoice.Firm.ID;

            //Dopisanie bieżącej daty do faktury
            invoice.DateOfCreation = DateTime.Now;

            //Dodanie faktury do bazy
            context.Invoices.Add(invoice);
            context.SaveChanges();

            //Pobranie faktury z bazy żeby mieć jej ID
            var invoiceFromBase = context.Invoices.ToList().LastOrDefault();


            //Tworzenie nowych produktów
            //Tych które stworzyliśmy klikając +
            //"NameNew PriceNew
            List<string> newProductsNames = new List<string>();
            List<string> newProductsPrices = new List<string>();
            int h = 1;
            while (Request.Form["NameNew" + h] != null)
            {
                newProductsNames.Add(Request.Form["NameNew" + h]);
                newProductsPrices.Add(Request.Form["PriceNew" + h]);
                Product product = new Product
                {
                    Name = Request.Form["NameNew" + h],
                    Price = int.Parse(Request.Form["PriceNew" + h])
                };
                context.Products.Add(product);
                context.SaveChanges();
                h++;
            }
            //NameNew1

            var qwe = 0;

            //Pobranie listy produktów z bazy
            var products = context.Products.ToList();

            //Tworzenie SoldProduct na podstawie danych z formularza
            //i danych z bazy produktów
            //AmountNew
            List<SoldProduct> soldProducts = new List<SoldProduct>();
            int i = 0;
            int t = 1;
            foreach (var item in products)
            {

                if (Request.Form["product" + i] != null)
                {
                    SoldProduct soldProduct = new SoldProduct
                    {
                        Count = int.Parse(Request.Form["product" + i]),
                        Product = products[i],
                        ProductID = products[i].ID,
                        Invoice = invoiceFromBase,
                        InvoiceID = invoiceFromBase.ID
                    };
                    context.SoldProducts.Add(soldProduct);
                    context.SaveChanges();
                    soldProducts.Add(soldProduct);
                }
                else if (Request.Form["AmountNew" + t] != null)
                {
                    SoldProduct soldProduct = new SoldProduct
                    {
                        Count = int.Parse(Request.Form["AmountNew" + t]),
                        Product = products[i],
                        ProductID = products[i].ID,
                        Invoice = invoiceFromBase,
                        InvoiceID = invoiceFromBase.ID
                    };
                    context.SoldProducts.Add(soldProduct);
                    context.SaveChanges();
                    soldProducts.Add(soldProduct);
                    t++;
                }


                i++;

            }


            //Tworzenie kwoty brutto do faktury
            double invoiceBruttoPrice = 0;

            //Zsumowanie wszystkich wartości produktów które dodaliśmy
            //w formularzu
            foreach (var item in soldProducts)
            {
                invoiceBruttoPrice += item.Count * item.Product.Price;
            }

            invoiceFromBase.BruttoPrice = invoiceBruttoPrice;

            //Jako że nasza faktura w zmiennej invoice
            //była pobrana z bazy danych i była śledzona(tracking)
            //to zapisujemy zmiany(zmienila się invoice.bruttoprice)
            context.SaveChanges();

            //Wszystkie dane z formularza są już w bazie danych
            //Przenosimy się do widoku faktury za pomocą Redirect
            return Redirect("/Home/Invoice/" + invoiceFromBase.ID);
        }


        /// <summary>
        /// Widok faktury, wyswietla dane z bazy danych
        /// wprowadzone w widoku Tworzenia zamowienia (Order)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Invoice(int id)
        {
            //Pobieramy fakture z bazy
            Invoice invoice = context.Invoices.Where(n => n.ID == id).FirstOrDefault();

            //Ładujemy do niej firme z bazy
            Firm firma = context.Firms.Where(n => n.ID == invoice.FirmID).FirstOrDefault();
            invoice.Firm = firma;

            //Przypisujemy dane
            ViewData["Faktura"] = invoice;

            //Dane produktów zakupionych
            //Pobieramy dane
            var soldProducts = context.SoldProducts.Where(n => n.InvoiceID == invoice.ID).ToList();

            //Zamieniamy je na HashSet
            var soldProducts2 = new HashSet<SoldProduct>();
            foreach (var item in soldProducts)
            {
                item.Product = context.Products.Where(n => n.ID == item.ProductID).FirstOrDefault();
                soldProducts2.Add(item);
            }

            //Przypisujemy dane
            ViewData["SoldProducts"] = soldProducts2;

            return View();
        }

        /// <summary>
        /// Widok tworzenia nowego produktu
        /// </summary>
        /// <returns></returns>
        public ActionResult NewProduct()
        {
            return View();
        }

        /// <summary>
        /// Metoda formularza w widoku tworzenia nowego produktu (NewProduct)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public RedirectResult CreateProductForm(Product product)
        {
            //Dodajemy produkt do bazy danych i zapisujemy zmiany
            context.Products.Add(product);
            context.SaveChanges();

            //Przenosimy się na stronę tworzenia zamówienia(Order)
            return Redirect("/Home/Order");
        }
    }
}