/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012  Joshua Michael Daly
 ********************************************************************
 * This file is part of RPGCode Express Version 1.
 *
 * RPGCode Express is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * RPGCode Express is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RPGCode Express.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RPGCode_Express.Classes.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class SerializableData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            string tempFilename = filename + ".tmp";

            FileInfo tempFileInfo = new FileInfo(tempFilename);

            if (tempFileInfo.Exists == true)
                tempFileInfo.Delete();

            FileStream stream = new FileStream(tempFilename, FileMode.Create);

            Save(stream);
            stream.Close();

            tempFileInfo.CopyTo(filename, true);
            tempFileInfo.Delete();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(stream, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="newType"></param>
        /// <returns></returns>
        public object Load(string filename, Type newType)
        {
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists == false)
                return System.Activator.CreateInstance(newType);

            FileStream stream = new FileStream(filename, FileMode.Open);
            object newObject = Load(stream, newType);

            stream.Close();
            return newObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="newType"></param>
        /// <returns></returns>
        public object Load(Stream stream, Type newType)
        {
            XmlSerializer serializer = new XmlSerializer(newType);
            object newObject = serializer.Deserialize(stream);

            return newObject;
        }
    }
}
