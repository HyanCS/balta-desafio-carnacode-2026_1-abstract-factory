// DESAFIO: Sistema de Pagamentos Multi-Gateway
// PROBLEMA: Uma plataforma de e-commerce precisa integrar com múltiplos gateways de pagamento
// (PagSeguro, MercadoPago, Stripe) e cada gateway tem componentes específicos (Processador, Validador, Logger)
// O código atual está muito acoplado e dificulta a adição de novos gateways

using System;
using Microsoft.VisualBasic;

namespace DesignPatternChallenge
{
    // Contexto: Sistema de pagamentos que precisa trabalhar com diferentes gateways
    // Cada gateway tem sua própria forma de processar, validar e logar transações
    #region Código Atual (Problema)
    public class PaymentService
    {
        private readonly IPaymentProviderFactory _factory;

        public PaymentService(IPaymentProviderFactory factory)
        {
            _factory = factory;
        }

        public void ProcessPayment(decimal amount, string cardNumber)
        {
            // Problema: Switch case gigante para cada gateway
            // Quando adicionar novo gateway, precisa modificar este método

            var validator = _factory.CreateValidateCardService();
            if (!validator.ValidateCard(cardNumber))
            {
                Console.WriteLine("Cartão inválido");
                return;
            }

            var processor = _factory.CreateProcessTransactionService();
            var result = processor.ProcessTransaction(amount, cardNumber);

            var logger = _factory.CreateLogService();
            logger.Log($"Transação processada: {result}");
        }
    }
    #endregion

    #region Componentes do PagSeguro
    public class PagSeguroClient
    {
        //Client implementation would be here, for example:
    }

    public sealed class PagSeguroPaymentFactory(PagSeguroClient client) : IPaymentProviderFactory
    {
        public IPaymentValidatorService CreateValidateCardService() => new PagSeguroValidatorService(client);
        public IPaymentProcessorService CreateProcessTransactionService() => new PagSeguroProcessorService(client);
        public IPaymentLoggerService CreateLogService() => new PagSeguroLoggerService(client);
    }

    public sealed class PagSeguroValidatorService(PagSeguroClient client) : IPaymentValidatorService
    {
        public bool ValidateCard(string cardNumber)
        {
            //Https calls would be implemented using the client provided in the constructor, for example:
            //client.ValidateCard(cardNumber);
            Console.WriteLine("PagSeguro: Validando cartão...");
            return cardNumber.Length == 16;
        }
    }

    public sealed class PagSeguroProcessorService(PagSeguroClient client) : IPaymentProcessorService
    {
        public string ProcessTransaction(decimal amount, string cardNumber)
        {
            //Https calls would be implemented using the client provided in the constructor, for example:
            //client.ProcessPayment(amount, cardNumber);
            Console.WriteLine($"PagSeguro: Processando R$ {amount}...");
            return $"PAGSEG-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }

    public sealed class PagSeguroLoggerService(PagSeguroClient client) : IPaymentLoggerService
    {
        public void Log(string message)
        {
            //Considering that the logs would be sent to an external service, the implementation would be done using the client provided in the constructor, for example:
            //client.SendLog(message);
            Console.WriteLine($"[PagSeguro Log] {DateTime.Now}: {message}");
        }
    }
    #endregion

    #region Componentes do MercadoPago

    public class MercadoPagoClient
    {
        //Client implementation would be here, for example:
    }
    public sealed class MercadoPagoPaymentFactory(MercadoPagoClient client) : IPaymentProviderFactory
    {
        public IPaymentValidatorService CreateValidateCardService() => new MercadoPagoValidatorService(client);
        public IPaymentProcessorService CreateProcessTransactionService() => new MercadoPagoProcessorService(client);
        public IPaymentLoggerService CreateLogService() => new MercadoPagoLoggerService(client);
    }
    public sealed class MercadoPagoValidatorService(MercadoPagoClient client) : IPaymentValidatorService
    {
        public bool ValidateCard(string cardNumber)
        {
            //Https calls would be implemented using the client provided in the constructor, for example:
            //client.ValidateCard(cardNumber);
            Console.WriteLine("MercadoPago: Validando cartão...");
            return cardNumber.Length == 16 && cardNumber.StartsWith("5");
        }
    }

    public sealed class MercadoPagoProcessorService(MercadoPagoClient client) : IPaymentProcessorService
    {
        public string ProcessTransaction(decimal amount, string cardNumber)
        {
            //Https calls would be implemented using the client provided in the constructor, for example:
            //client.ProcessPayment(amount, cardNumber);
            Console.WriteLine($"MercadoPago: Processando R$ {amount}...");
            return $"MP-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }

    public sealed class MercadoPagoLoggerService(MercadoPagoClient client) : IPaymentLoggerService
    {
        public void Log(string message)
        {
            //Considering that the logs would be sent to an external service, the implementation would be done using the client provided in the constructor, for example:
            //client.SendLog(message);
            Console.WriteLine($"[MercadoPago Log] {DateTime.Now}: {message}");
        }
    }
    #endregion

    #region Componentes do Stripe 
    public sealed class StripeClient
    {
        //Client implementation would be here, for example:
    }

    public sealed class StripePaymentFactory(StripeClient client) : IPaymentProviderFactory
    {
        public IPaymentValidatorService CreateValidateCardService() => new StripeValidatorService(client);
        public IPaymentProcessorService CreateProcessTransactionService() => new StripeProcessorService(client);
        public IPaymentLoggerService CreateLogService() => new StripeLoggerService(client);
    }

    public sealed class StripeValidatorService(StripeClient client) : IPaymentValidatorService
    {
        public bool ValidateCard(string cardNumber)
        {
            //Calls to the client would be implemented using the client provided in the constructor, for example:
            //client.ValidateCard(cardNumber);
            Console.WriteLine("Stripe: Validando cartão...");
            return cardNumber.Length == 16 && cardNumber.StartsWith("4");
        }
    }

    public sealed class StripeProcessorService(StripeClient client) : IPaymentProcessorService
    {
        public string ProcessTransaction(decimal amount, string cardNumber)
        {
            //Calls to the client would be implemented using the client provided in the constructor, for example:
            //client.ProcessPayment(amount, cardNumber);
            Console.WriteLine($"Stripe: Processando ${amount}...");
            return $"STRIPE-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }

    public sealed class StripeLoggerService(StripeClient client) : IPaymentLoggerService
    {
        public void Log(string message)
        {
            //Considering that the logs would be sent to an external service, the implementation would be done using the client provided in the constructor, for example:
            //client.SendLog(message);
            Console.WriteLine($"[Stripe Log] {DateTime.Now}: {message}");
        }
    }
    #endregion

    #region Interfaces
    public interface IPaymentLoggerService
    {
        void Log(string message);
    }

    public interface IPaymentProcessorService
    {
        string ProcessTransaction(decimal amount, string cardNumber);
    }

    public interface IPaymentProviderFactory
    {
        IPaymentValidatorService CreateValidateCardService();
        IPaymentProcessorService CreateProcessTransactionService();
        IPaymentLoggerService CreateLogService();
    }

    public interface IPaymentValidatorService
    {
        bool ValidateCard(string cardNumber);
    }
    #endregion

    #region Programa de Teste
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Pagamentos ===\n");

            // Problema: Cliente precisa saber qual gateway está usando
            // e o código de processamento está todo acoplado
            var factory = BuildFactory(provider: "Stripe");
            var paymentService = new PaymentService(factory);
            paymentService.ProcessPayment(150.00m, "1234567890123456");

            Console.WriteLine();

            var factory2 = BuildFactory(provider: "MercadoPago");
            var paymentService2 = new PaymentService(factory2);
            paymentService2.ProcessPayment(200.00m, "5234567890123456");
            Console.WriteLine();

            // Pergunta para reflexão:
            // - Como adicionar um novo gateway sem modificar PaymentService?
            // - Como garantir que todos os componentes de um gateway sejam compatíveis entre si?
            // - Como evitar criar componentes de gateways diferentes acidentalmente?
        }

        public static IPaymentProviderFactory BuildFactory(string provider)
        {
            return provider switch
            {
                "Stripe" => new StripePaymentFactory(new StripeClient()),
                "PagSeguro" => new PagSeguroPaymentFactory(new PagSeguroClient()),
                "MercadoPago" => new MercadoPagoPaymentFactory(new MercadoPagoClient()),
                _ => throw new NotSupportedException($"Provider inválido: {provider}")
            };
        }
    }
}

    #endregion
