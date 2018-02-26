﻿namespace bgTeam.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс для получения информации из базы данных
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Выполняет запрос к базе данных и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="obj">Объект с запросом к базе данных</param>
        /// <returns></returns>
        T Get<T>(ISqlObject obj);

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="obj">Объект с запросом к базе данных</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(ISqlObject obj);

        /// <summary>
        /// Выполняет запрос к базе данных и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <returns></returns>
        T Get<T>(string sql, object param = null);

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string sql, object param = null);

        /// <summary>
        /// Выполняет запрос к базе данных на основе предиката и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="predicate">Предикат</param>
        /// <returns></returns>
        T Get<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных на основе предиката и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="predicate">Предикат</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="obj">Объект с запросом к базе данных</param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(ISqlObject obj);

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="obj">Объект с запросом к базе данных</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>(ISqlObject obj);

        /// <summary>
        /// Выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(string sql, object param = null);

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>(string sql, object param = null);

        /// <summary>
        /// Выполняет запрос к базе данных на основе предиката и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="predicate">Предикат</param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> predicate = null)
            where T : class;

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных на основе предиката и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="predicate">Предикат</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null)
            where T : class;
    }
}
