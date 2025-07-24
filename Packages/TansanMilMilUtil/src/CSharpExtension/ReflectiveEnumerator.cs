using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ReflectiveEnumerator
    {
        /// <summary>
        /// 指定された型のすべてのサブクラスのインスタンスを取得します。
        /// </summary>
        /// <typeparam name="T">基底クラスの型</typeparam>
        public static IEnumerable<T> GetAllSubClasses<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            Assembly assembly = Assembly.GetAssembly(typeof(T));

            foreach (Type type in assembly
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)assembly.CreateInstance(
                    type.FullName,
                    ignoreCase: false,
                    bindingAttr: BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding,
                    binder: null,
                    args: constructorArgs,
                    culture: null,
                    activationAttributes: null
                ));
            }
            return objects;
        }
    }
}
