using System;
using System.Globalization;
using System.Xml.Linq;

namespace Questao1
{
    class ContaBancaria {
        private int numero;
        private string titular;
        private double saldo;

        public ContaBancaria(int numero, string titular, double depositoInicial = 0) { 
            this.numero = numero;
            this.titular = titular;
            this.saldo = depositoInicial;
        }

        public void Deposito(double quantia)
        {
            if(quantia<0)
            {
                throw new Exception("Informe um valor positivo a ser depositado");
            }
            this.saldo += quantia;
        }

        public void Saque(double quantia)
        {
            if (quantia < 0)
            {
                throw new Exception("Informe um valor positivo a ser sacado");
            }
            const double taxa = 3.5;
            this.saldo = this.saldo - quantia - taxa;
        }

        public void SetTitular(string titular)
        {
            this.titular = titular;
        }

        public override string ToString()
        {
            return "Conta " + this.numero + ", Titular: " + this.titular + ", Saldo: $ " + this.saldo.ToString("F2");
        }
    }
}