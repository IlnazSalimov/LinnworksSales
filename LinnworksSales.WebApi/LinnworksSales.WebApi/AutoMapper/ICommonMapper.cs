using System;

namespace LinnworksSales.WebApi.Models
{
    public interface ICommonMapper
    {
        /// <summary>
        /// Выполняет отображение исходного объекта в новый объект назначения.</summary>
        /// <param name="source">Объект-источник данных для отображения</param>
        /// <param name="sourceType">Тип объекта-источника(использование)</param>
        /// <param name="destinationType">Тип объекта-назначения(создание)</param>
        /// <returns>Отображенный объект назначения</returns>
        object Map(object source, Type sourceType, Type destinationType);

        /// <summary>
        /// Выполняет отображение исходного объекта в существующий объект назначения.</summary>
        /// <param name="source">Объект-источник данных для отображения</param>
        /// <param name="destination">Объект назначения</param>
        /// <returns>Отображенный объект назначения</returns>
        object Map(object source, object destination);

        /// <summary>
        /// Выполняет приведение исходного объекта в указанный тип.</summary>
        /// <typeparam name="T">Тип выходного объекта.</typeparam>
        /// <param name="source">Модель данных для приведения.</param>
        /// <returns>Результат выполнения приведения.</returns>
        T Map<T>(object source) where T : class;
    }
}
