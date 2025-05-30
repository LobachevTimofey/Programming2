using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task16
{
    public struct PhoneCall
    {
        public int Time;
        public double Rate;
        public double Cost 
        {
            get => (float)Math.Round((Time / 60f) * Rate, 2);
        }
        public PhoneCall(int time, double rate)
        {
            if (time <= 0 || time % 1 != 0)
                throw new ArgumentException("Время разговора должно целым, положительным числом");
            if (rate <= 0)
                throw new ArgumentException("Стоимость тарифа должна быть положительным числом");

            Time = time;
            Rate = rate;
        }
        public override string ToString()
        {
            return $"Разговор: {Time} с по {Rate} руб./мин.";
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Нельзя сравнивать с null.");

            if (!(obj is PhoneCall))
                throw new ArgumentException("Объект не является экземпляром PhoneCall.", nameof(obj));

            var other = (PhoneCall)obj;
            return Time == other.Time && Rate == other.Rate;
        }
        public override int GetHashCode()=> Time.GetHashCode() ^ Rate.GetHashCode();
        public static PhoneCall operator +(PhoneCall firstCall, PhoneCall secondCall)
        {
            if (firstCall.Rate != secondCall.Rate)
                throw new InvalidOperationException("Нельзя складывать разговоры с разными тарифами.");

            return new PhoneCall(firstCall.Time + secondCall.Time, firstCall.Rate);
        }
        public static PhoneCall operator *(PhoneCall call, double multiplier)
        {
            if (multiplier <= 0)
                throw new ArgumentException("Множитель должен быть положительным числом.");

            return new PhoneCall(call.Time, call.Rate * multiplier);
        }

    }
}
