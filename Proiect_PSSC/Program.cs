using Proiect_PSSC.Workflow;
using Proiect_PSSC.Models;
using Proiect_PSSC.Commands;

class Program
{
    static void Main()
    {
        // citirea produselor si salvarea lor intr-o variabila 
        var products = ReadProducts().ToArray();

        // citirea datelor pentru facturare 
        BillingDetails();

        // se creeaza comanda ShipOrderCartCommands cu produsele citite de la tastatura 
        ShipOrderCartCommands command = new(products);
        PayOrderCartWorkflow workflow = new();

        // se apeleaza metoda Execute din workflow cu comanda definita anterior 
        var result = workflow.Execute(command, (productName) => true);

        result.Match(
            whenIShippedCartFailedEvent: @event =>
            {
                Console.WriteLine($"Shipment failed: {@event.Reason}");
                return @event;
            },
            whenIShippedCartSuccessfulEvent: @event =>
            {
                Console.WriteLine($"Shipment successful: {@event.Reason}");
                return @event;
            }
            );

        Console.WriteLine("Done");
    }

    // metoda pentru citirea produselor de la tastatura
    private static List<UnvalidatedOrderCartProduct> ReadProducts()
    {
        List<UnvalidatedOrderCartProduct> listOfProducts = new();
        do
        {
            var productName = ReadValue("Product name: ");
            if (string.IsNullOrEmpty(productName))
            {
                break;
            }

            var price = ReadValue("Price: ");
            if (string.IsNullOrEmpty(price))
            {
                break;
            }

            listOfProducts.Add(new(productName, price));
        } while (true);
        return listOfProducts;
    }

    private static string? ReadValue(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    // metoda pentru citirea datelor de facturare de la tastatura 
    // datele de facturare sunt asociate unui order intotdeauna 
    public static void BillingDetails()
    {
        Order order = new Order();
        // se introduc detaliile comenzii, apoi sunt validate utilizând metodele ValidateStringInput, ValidatePhoneNumber și ValidateOrderPrice
        try
        {
            Console.Write("Introduceti persoana de contact: ");
            order = order with { ContactPerson = ValidateStringInput() };

            Console.Write("Introduceti numarul de telefon: ");
            order = order with { PhoneNumber = ValidatePhoneNumber() };

            Console.Write("Introduceti adresa de livrare: ");
            order = order with { DeliveryAddress = ValidateStringInput() };
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Eroare: {ex.Message}. Va rugam sa incercati din nou.");
           
        }
    }

    // metode de validare 

    private static string ValidateStringInput()
    {
        string input = Console.ReadLine();

        //se verifica ca stringul nu este gol si contine doar litere
        if (string.IsNullOrWhiteSpace(input) || !input.All(char.IsLetter))
        {
            Console.WriteLine("Eroare: Introduceti o valoare valida. Nu trebuie sa fie goala si sa contina doar litere.");
            return ValidateStringInput();
        }
        return input;
    }

    private static string ValidatePhoneNumber()
    {
        // se obtine input-ul de la utilizator
        string input = Console.ReadLine();

   
        // verificam dacă input-ul contine doar cifre
        if (!input.All(char.IsDigit))
        {
            Console.WriteLine("Eroare: Introduceti un numar de telefon valid. Numai cifre sunt permise.");
            return ValidatePhoneNumber();
        }
        return input;
    }
}


