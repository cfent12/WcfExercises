using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 서비스 계약의 기본 구현이 포함되어 있습니다.
namespace WcfServiceLibrary
{
    public class CalculatorService : ICalculator 
    {
        public string Hello(string greeting)
        {
            throw new NotImplementedException();
        }

        public void Hello2(string greeting)
        {
            throw new NotImplementedException();
        }

        public string Hello3(string greeting)
        {
            throw new NotImplementedException();
        }

        public string FaultContractSampleMethod(string msg)
        {
            Console.WriteLine("Client said: " + msg);
            // Generate intermittent error behavior.
            Random rnd = new Random(DateTime.Now.Millisecond);
            int test = rnd.Next(5);
            if (test % 2 != 0)
                return "The service greets you: " + msg;
            else
                throw new FaultException<GreetingFault>(new GreetingFault("A Greeting error occurred. You said: " + msg));
        }

        public double Add(double n1, double n2)
        {
            double result = n1 + n2;
            Console.WriteLine("Received Add({0},{1})", n1, n2);
            // Code added to write output to the console window.
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Subtract(double n1, double n2)
        {
            double result = n1 - n2;
            Console.WriteLine("Received Subtract({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Multiply(double n1, double n2)
        {
            double result = n1 * n2;
            Console.WriteLine("Received Multiply({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Divide(double n1, double n2)
        {
            double result = n1 / n2;
            Console.WriteLine("Received Divide({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }
    }

    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 클래스 이름 "Service1"을 변경할 수 있습니다.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }

    // Service class which implements a duplex service contract.
    // Use an InstanceContextMode of PerSession to store the result
    // An instance of the service will be bound to each duplex session
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CalculatorDuplexService : ICalculatorDuplex
    {
        ICalculatorDuplexCallback callback = null;
        string equation;
        double result;
    
        public CalculatorDuplexService()
        {
            callback = OperationContext.Current.GetCallbackChannel<ICalculatorDuplexCallback>();
        }
    
        public void Clear()
        {
            callback.Equation(equation + " = " + result.ToString());
            result = 0.0D;
            equation = result.ToString();
        }
    
        public void AddTo(double n)
        {
            result += n;
            equation += " + " + n.ToString();
            callback.Equals(result);
        }
    
        public void SubtractFrom(double n)
        {
            result -= n;
            equation += " - " + n.ToString();
            callback.Equals(result);
        }
    
        public void MultiplyBy(double n)
        {
            result *= n;
            equation += " * " + n.ToString();
            callback.Equals(result);
        }
    
        public void DivideBy(double n)
        {
            result /= n;
            equation += " / " + n.ToString();
            callback.Equals(result);
        }
    }
}
