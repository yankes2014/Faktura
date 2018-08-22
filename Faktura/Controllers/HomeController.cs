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
        public RedirectResult CreateInvoice(Firm firma)
        {
            //Dodanie firmy do bazy firm
            Firm firm = firma;

            context.Firms.Add(firm);
            context.SaveChanges();

            //Tworzymy fakture
            Invoice invoice = new Invoice();

            //Dopisanie firmy do faktury
            invoice.Firm = context.Firms.Where(n => n.NIP == firm.NIP).FirstOrDefault();
            invoice.FirmID = invoice.Firm.ID;

            //Dopisanie bieżącej daty do faktury
            invoice.DateOfCreation = DateTime.Now;

            //Dodanie faktury do bazy
            context.Invoices.Add(invoice);
            context.SaveChanges();

            //Pobranie faktury z bazy żeby mieć jej ID
            //pobieramy ostatnią na ślepo co czasami jest niepoprawne
            //można by w tabeli faktura stworzyć unikatowe pole
            //np sprowadzając CreationDate do stringa
            //i szyfrujac jakimś algorytmem z kluczem
            //i wtedy można by wziąć z bazy 
            //Invoice.Where(n=>n.Zaszyfrowane == invoice.Zaszyfrowane).FirstOrDefault
            var invoiceFromBase = context.Invoices.ToList().LastOrDefault();

            //Pobranie listy produktów z bazy
            var products = context.Products.ToList();

            //Tworzenie SoldProduct na podstawie danych z formularza
            //i danych z bazy produktów
            List<SoldProduct> soldProducts = new List<SoldProduct>();
            int i = 0;
            foreach (var item in products)
            {
                if (int.Parse(Request.Form["product" + i]) != 0)
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