using GrandCheese.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.Data
{
    public class KSimpleDate
    {
        public static int getWeekday(int year, int month, int day)
        {
            int zellerMonth;
            int zellerYear;

            if (month < 3)
            { // 월값이 3보다 작으면
                zellerMonth = month + 12; // 월 + 12
                zellerYear = year - 1; // 연 - 1
            }
            else
            {
                zellerMonth = month;
                zellerYear = year;
            }

            int computation = day + (26 * (zellerMonth + 1)) / 10 + zellerYear +
                    zellerYear / 4 + 6 * (zellerYear / 100) +
                    zellerYear / 400;

            int dayOfWeek = computation % 7; // 토=0, 일=1
            dayOfWeek--;

            if (dayOfWeek == -1)
                return 6;
            else
                return dayOfWeek;
        }

        public static int getLastDay(int year, int month)
        {
            switch (month)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
                        return 29;
                    else
                        return 28;
                default:
                    return 31;
            }
        }

        public static void write_KSimpleDate(Packet p, int year, int month, int day, int weekday)
        {
            p.WriteShort((short)year);
            p.Write((byte)month);
            p.Write((byte)day);
            p.Write((byte)weekday);
        }

        public static void write_KSimpleDate(Packet p, int year, int month, int day)
        {
            write_KSimpleDate(p, year, month, day, getWeekday(year, month, day));
        }
    }
}
