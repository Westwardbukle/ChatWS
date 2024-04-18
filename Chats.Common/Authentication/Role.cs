using System.ComponentModel.DataAnnotations;
using Chats.Common.Models;

namespace Chats.Common.Authentication
{
    /// <summary>
    /// Модель роли пользователей системы.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Инициализирует экземпляр класса <see cref="Role"/>
        /// </summary>
        public Role()
        {
        }

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="Role"/> с указанием имени роли
        /// </summary>
        /// <param name="name">Имя роли</param>
        public Role(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя роли.
        /// </summary>
        [MaxLength(100, ErrorMessage = "Имя роли не должно превышать 100 символов")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Имя роли может содержать только латинские буквы и цифры")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Приоритет роли, по убыванию важности. Например 1 - самая важная роль,
        /// которая будет отображаться выше всех остальных, а 10 - наименее важная роль
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Описание роли
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Доступы роли к UnO
        /// </summary>
        public CrudAccess UnoAccess { get; set; } = default!;

        /// <summary>
        /// Проверяет идентичность объектов, сравнивая имена ролей без учёта регистра
        /// </summary>
        /// <param name="obj">Объект сравнения</param>
        /// <returns></returns>
        /// <remarks>Внимание, метод не поддерживается entity framework 6!</remarks>
        public override bool Equals(object? obj)
        {
            if (obj is null or not Role)
                return false;

            return Name.ToUpper() == ((Role)obj).Name.ToUpper();
        }

        /// <summary>
        /// Получение хэш-кода
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.ToUpper().GetHashCode();
        }
    }
}