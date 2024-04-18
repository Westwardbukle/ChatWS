using Chats.Common.Authentication;

namespace Chats.Common.Models
{
    /// <summary>
    /// Модель краткой информации о пользователе системы
    /// </summary>
#warning Дублирование кода
    public class UserInfo
    {
        /// <summary>
        /// Инициализирует экземпляр класса <see cref="UserInfo"/> с указанием ФИО пользователя
        /// </summary>
        /// <param name="lastName">Фамилия пользователя</param>
        /// <param name="firstName">Имя пользователя</param>
        /// <param name="middleName">Отчество пользователя</param>
        public UserInfo(string lastName, string firstName, string? middleName = null)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Активен ли пользователь
        /// </summary>
        public bool IsActive => !DeactivationDate.HasValue;

        /// <summary>
        /// Дата деактивации
        /// </summary>
        public DateTime? DeactivationDate { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; } = default!;

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// ФИО пользователя в формате Иванов Иван Иванович
        /// </summary>
        public string Fio
        {
            get
            {
                var fio = string.Empty;

                if (!string.IsNullOrEmpty(LastName))
                    fio += $"{LastName}";

                if (!string.IsNullOrEmpty(FirstName))
                    fio += $" {FirstName}";

                if (!string.IsNullOrEmpty(MiddleName))
                    fio += $" {MiddleName}";

                return fio;
            }
        }

        /// <summary>
        /// ФИО пользователя в формате Иванов И. И.
        /// </summary>
        public string FioShort
        {
            get
            {
                var fio = string.Empty;

                if (!string.IsNullOrEmpty(LastName))
                    fio += $"{LastName}";

                if (!string.IsNullOrEmpty(FirstName))
                    fio += $" {FirstName.First()}.";

                if (!string.IsNullOrEmpty(MiddleName))
                    fio += $" {MiddleName.First()}.";

                return fio;
            }
        }

        /// <summary>
        /// Роль пользователя, которая имеет наиболее высокий приоритет
        /// </summary>
        public string DisplayRoleString => Roles.MinBy(p => p.Priority)?.Description ?? "Без роли";

        /// <summary>
        /// Список ролей пользователя
        /// </summary>
        public IEnumerable<Role> Roles { get; set; } = Enumerable.Empty<Role>();
    }
}