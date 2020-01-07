using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BS_Utils.Utilities
{
    public static class ReflectionUtil
    {
        private const BindingFlags _allBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;


        /// <summary>
        /// Sets a field on the target object. <paramref name="targetType"/> specifies the <see cref="Type"/> the field belongs to. 
        /// </summary>
        /// <param name="obj">the object instance</param>
        /// <param name="fieldName">the field to set</param>
        /// <param name="value">the value to set it to</param>
        /// <param name="targetType">the object <see cref="Type"/> the field belongs to</param>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
        /// <exception cref="ArgumentException">thrown when <paramref name="obj"/> isn't assignable as <paramref name="targetType"/></exception>
		public static void SetField(this object obj, string fieldName, object value, Type targetType)
        {
            var prop = targetType.GetField(fieldName, _allBindingFlags);
            if (prop == null)
                throw new InvalidOperationException($"{fieldName} is not a member of {targetType.Name}");
            prop.SetValue(obj, value);
        }

        /// <summary>
        /// Gets the value of a field. <paramref name="targetType"/> specifies the <see cref="Type"/> the field belongs to.
        /// </summary>
        /// <param name="obj">the object instance to pull from</param>
        /// <param name="fieldName">the name of the field to read</param>
        /// <param name="targetType">the object <see cref="Type"/> the field belongs to</param>
        /// <returns>the value of the field</returns>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
        /// <exception cref="ArgumentException">thrown when <paramref name="obj"/> isn't assignable as <paramref name="targetType"/></exception>
        public static object GetField(this object obj, string fieldName, Type targetType)
        {
            var prop = targetType.GetField(fieldName, _allBindingFlags);
            if (prop == null)
                throw new InvalidOperationException($"{fieldName} is not a member of {targetType.Name}");
            return prop.GetValue(obj);
        }

        /// <summary>
        /// Sets a private field on the target object. <paramref name="targetType"/> specifies the <see cref="Type"/> the field belongs to. 
        /// </summary>
        /// <param name="obj">the object instance</param>
        /// <param name="fieldName">the field to set</param>
        /// <param name="value">the value to set it to</param>
        /// <param name="targetType">the object <see cref="Type"/> the field belongs to</param>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
        /// <exception cref="ArgumentException">thrown when <paramref name="obj"/> isn't assignable as <paramref name="targetType"/></exception>
		public static void SetPrivateField(this object obj, string fieldName, object value, Type targetType)
        {
            var prop = targetType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop == null)
                throw new InvalidOperationException($"{fieldName} is not a member of {targetType.Name}");
            prop.SetValue(obj, value);
        }

        /// <summary>
        /// Gets the value of a private field. <paramref name="targetType"/> specifies the <see cref="Type"/> the field belongs to.
        /// </summary>
        /// <typeparam name="T">the type of the field (result casted)</typeparam>
        /// <param name="obj">the object instance to pull from</param>
        /// <param name="fieldName">the name of the field to read</param>
        /// <param name="targetType">the object <see cref="Type"/> the field belongs to</param>
        /// <returns>the value of the field</returns>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
        /// <exception cref="ArgumentException">thrown when <paramref name="obj"/> isn't assignable as <paramref name="targetType"/></exception>
        public static T GetPrivateField<T>(this object obj, string fieldName, Type targetType)
        {
            var prop = targetType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop == null)
                throw new InvalidOperationException($"{fieldName} is not a member of {targetType.Name}");
            var value = prop.GetValue(obj);
            return (T)value;
        }


        /// <summary>
        /// Sets the value of a property on the target object. <paramref name="targetType"/> specifies the <see cref="Type"/> the property belongs to. 
        /// </summary>
        /// <param name="obj">the object instance</param>
        /// <param name="propertyName">the property to set</param>
        /// <param name="value">the value to set it to</param>
        /// <param name="targetType">the object <see cref="Type"/> the property belongs to</param>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="propertyName"/> is not a member of <paramref name="obj"/></exception>
        /// <exception cref="ArgumentException">thrown when <paramref name="obj"/> isn't assignable as <paramref name="targetType"/></exception>
        public static void SetProperty(this object obj, string propertyName, object value, Type targetType)
        {
            var prop = targetType.GetProperty(propertyName, _allBindingFlags);
            if (prop == null)
                throw new InvalidOperationException($"{propertyName} is not a member of {targetType.Name}");
            prop.SetValue(obj, value);
        }

        /// <summary>
        /// Gets the value of a property. <paramref name="targetType"/> specifies the <see cref="Type"/> the field belongs to.
        /// </summary>
        /// <param name="obj">the object instance to pull from</param>
        /// <param name="propertyName">the name of the property to read</param>
        /// <param name="targetType">the object <see cref="Type"/> the property belongs to</param>
        /// <returns>the value of the property</returns>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="propertyName"/> is not a member of <paramref name="obj"/></exception>
        /// <exception cref="ArgumentException">thrown when <paramref name="obj"/> isn't assignable as <paramref name="targetType"/></exception>
        public static object GetProperty(this object obj, string propertyName, Type targetType)
        {
            var prop = targetType.GetProperty(propertyName, _allBindingFlags);
            if (prop == null)
                throw new InvalidOperationException($"{propertyName} is not a member of {targetType.Name}");
            var value = prop.GetValue(obj);
            return value;
        }


        #region Overloads
        /// <summary>
        /// Sets a private field on the target object. 
        /// </summary>
        /// <param name="obj">the object instance</param>
        /// <param name="fieldName">the field to set</param>
        /// <param name="value">the value to set it to</param>
        /// <param name="targetType">the object <see cref="Type"/> the field belongs to</param>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
		public static void SetPrivateField(this object obj, string fieldName, object value) => obj.SetPrivateField(fieldName, value, obj.GetType());


        /// <summary>
        /// Gets the value of a private field.
        /// </summary>
        /// <typeparam name="T">the type of te field (result casted)</typeparam>
        /// <param name="obj">the object instance to pull from</param>
        /// <param name="fieldName">the name of the field to read</param>
        /// <returns>the value of the field</returns>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
        public static T GetPrivateField<T>(this object obj, string fieldName) => obj.GetPrivateField<T>(fieldName, obj.GetType());

        /// <summary>
        /// Sets a (potentially) private field on the target object.
        /// </summary>
        /// <param name="obj">the object instance</param>
        /// <param name="fieldName">the field to set</param>
        /// <param name="value">the value to set it to</param>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
        public static void SetField(this object obj, string fieldName, object value) => obj.SetField(fieldName, value, obj.GetType());

        /// <summary>
        /// Gets the value of an object's field.
        /// </summary>
        /// <param name="obj">the object instance to pull from</param>
        /// <param name="fieldName">the name of the field to read</param>
        /// <returns>the value of the field</returns>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
        public static object GetField(this object obj, string fieldName) => GetField(obj, fieldName, obj.GetType());


        /// <summary>
        /// Gets the value of a field. <paramref name="targetType"/> specifies the <see cref="Type"/> the field belongs to.
        /// </summary>
        /// <typeparam name="T">the type of the field (result casted)</typeparam>
        /// <param name="obj">the object instance to pull from</param>
        /// <param name="fieldName">the name of the field to read</param>
        /// <param name="targetType">the object <see cref="Type"/> the field belongs to</param>
        /// <returns>the value of the field</returns>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
        /// <exception cref="ArgumentException">thrown when <paramref name="obj"/> isn't assignable as <paramref name="targetType"/></exception>
        public static T GetField<T>(this object obj, string fieldName, Type targetType) => (T)obj.GetField(fieldName, targetType);


        /// <summary>
        /// Gets the casted value of a field.
        /// </summary>
        /// <typeparam name="T">the type of the field (result casted)</typeparam>
        /// <param name="obj">the object instance to pull from</param>
        /// <param name="fieldName">the name of the field to read</param>
        /// <returns>the value of the field</returns>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="fieldName"/> is not a member of <paramref name="obj"/></exception>
        public static T GetField<T>(this object obj, string fieldName) => (T)GetField(obj, fieldName, obj.GetType());

        /// <summary>
        /// Sets the value of a property on the target object.
        /// </summary>
        /// <param name="obj">the object instance</param>
        /// <param name="propertyName">the property to set</param>
        /// <param name="value">the value to set it to</param>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="propertyName"/> is not a member of <paramref name="obj"/></exception>
        public static void SetProperty(this object obj, string propertyName, object value) => obj.SetProperty(propertyName, value, obj.GetType());

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <param name="obj">the object instance to pull from</param>
        /// <param name="propertyName">the name of the property to read</param>
        /// <returns>the value of the property</returns>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="propertyName"/> is not a member of <paramref name="obj"/></exception>
        public static object GetProperty(this object obj, string propertyName) => obj.GetProperty(propertyName, obj.GetType());

        /// <summary>
        /// Gets the casted value of a property.
        /// </summary>
        /// <typeparam name="T">they type of the property</typeparam>
        /// <param name="obj">the object instance to pull from</param>
        /// <param name="propertyName">the name of the property to read</param>
        /// <returns>the value of the property</returns>
        /// <exception cref="InvalidOperationException">thrown when <paramref name="propertyName"/> is not a member of <paramref name="obj"/></exception>
        public static T GetProperty<T>(this object obj, string propertyName) => (T)GetProperty(obj, propertyName);

        #endregion



        //Invokes a (static?) private method with name "methodName" and params "methodParams", returns an object of the specified type
        public static T InvokeMethod<T>(this object obj, string methodName, params object[] methodParams) => (T)InvokeMethod(obj, methodName, methodParams);

        //Invokes a (static?) private method with name "methodName" and params "methodParams"
        public static object InvokeMethod(this object obj, string methodName, params object[] methodParams)
        {
            MethodInfo method = obj.GetType().GetMethod(methodName, _allBindingFlags);
            if (method == null)
                throw new InvalidOperationException($"{methodName} is not a member of {obj.GetType().Name}");
            return method.Invoke(obj, methodParams);
        }

        //Returns a constructor with the specified parameters to the specified type or object
        public static object InvokeConstructor(this object obj, params object[] constructorParams)
        {
            Type[] types = new Type[constructorParams.Length];
            for (int i = 0; i < constructorParams.Length; i++) types[i] = constructorParams[i].GetType();
            return (obj is Type ? (Type)obj : obj.GetType())
                .GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, types, null)
                .Invoke(constructorParams);
        }

        //Returns a Type object which can be used to invoke static methods with the above helpers
        public static Type GetStaticType(string clazz)
        {
            return Type.GetType(clazz);
        }

        //Returns a list (of strings) of the names of all loaded assemblies
        public static IEnumerable<Assembly> ListLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        //Returns a list of all loaded namespaces
        //TODO: Check up on time complexity here, could potentially be parallelized
        public static IEnumerable<string> ListNamespacesInAssembly(Assembly assembly)
        {
            IEnumerable<string> ret = Enumerable.Empty<string>();
            ret = ret.Concat(assembly.GetTypes()
                    .Select(t => t.Namespace)
                    .Distinct()
                    .Where(n => n != null));
            return ret.Distinct();
        }

        //Returns a list of classes in a namespace
        //TODO: Check up on time complexity here, could potentially be parallelized
        public static IEnumerable<string> ListClassesInNamespace(string ns)
        {
            //For each loaded assembly
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                //If the assembly contains the desired namespace
                if (assembly.GetTypes().Where(t => t.Namespace == ns).Any())
                {
                    //Select the types we want from the namespace and return them
                    return assembly.GetTypes()
                        .Where(t => t.IsClass)
                        .Select(t => t.Name);
                }
            }
            return null;
        }

        //(Created by taz?) Copies a component to a destination object, keeping all its field values?
        public static Behaviour CopyComponent(Behaviour original, Type originalType, Type overridingType, GameObject destination)
        {
            Behaviour copy = null;

            try
            {
                copy = destination.AddComponent(overridingType) as Behaviour;
            }
            catch (Exception)
            {

            }

            copy.enabled = false;

            //Copy types of super classes as well as our class
            Type type = originalType;
            while (type != typeof(MonoBehaviour))
            {
                CopyForType(type, original, copy);
                type = type.BaseType;
            }

            copy.enabled = true;
            return copy;
        }

        //(Created by taz?) Copies a Component of Type type, and all its fields
        private static void CopyForType(Type type, Component source, Component destination)
        {
            FieldInfo[] myObjectFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField);

            foreach (FieldInfo fi in myObjectFields)
            {
                fi.SetValue(destination, fi.GetValue(source));
            }
        }
    }
}
