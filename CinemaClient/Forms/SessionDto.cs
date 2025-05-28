using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaClient.Models
{

    /// <summary>
    /// Data Transfer Object (DTO) для представления информации о киносеансе
    /// </summary>
    public class SessionDto
    {
        /// <summary>
        /// Уникальный идентификатор сеанса
        /// </summary>
        public int SessionId { get; set; }

        /// <summary>
        /// Идентификатор фильма
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// Название фильма
        /// </summary>
        public string MovieTitle { get; set; }

        /// <summary>
        /// Название кинозала
        /// </summary>
        public string HallName { get; set; }

        /// <summary>
        /// Дата проведения сеанса
        /// </summary>
        public DateTime SessionDate { get; set; }

        /// <summary>
        /// Время начала сеанса (в формате "HH:mm")
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Время окончания сеанса (в формате "HH:mm")
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Базовая цена билета
        /// </summary>
        public decimal BasePrice { get; set; } = 280m;

        /// <summary>
        /// Форматированная дата сеанса (дд.ММ.гггг)
        /// </summary>
        public string FormattedDate => SessionDate.ToString("dd.MM.yyyy");

        /// <summary>
        /// Полная информация о времени сеанса
        /// </summary>
        public string FullTimeInfo => $"{StartTime} - {EndTime}";

        /// <summary>
        /// Полное описание сеанса для отображения
        /// </summary>
        public string FullSessionInfo =>
            $"{MovieTitle} | {FormattedDate} {FullTimeInfo} | {HallName}";

        /// <summary>
        /// Проверяет, валиден ли объект (заполнены основные поля)
        /// </summary>
        public bool IsValid()
        {
            return SessionId > 0 &&
                   MovieId > 0 &&
                   !string.IsNullOrWhiteSpace(MovieTitle) &&
                   !string.IsNullOrWhiteSpace(HallName) &&
                   !string.IsNullOrWhiteSpace(StartTime) &&
                   !string.IsNullOrWhiteSpace(EndTime);
        }

        /// <summary>
        /// Преобразует время начала в DateTime
        /// </summary>
        public DateTime GetStartDateTime()
        {
            if (TimeSpan.TryParse(StartTime, out var time))
            {
                return SessionDate.Date.Add(time);
            }
            throw new FormatException("Некорректный формат времени начала");
        }

        /// <summary>
        /// Преобразует время окончания в DateTime
        /// </summary>
        public DateTime GetEndDateTime()
        {
            if (TimeSpan.TryParse(EndTime, out var time))
            {
                return SessionDate.Date.Add(time);
            }
            throw new FormatException("Некорректный формат времени окончания");
        }
    }
}
