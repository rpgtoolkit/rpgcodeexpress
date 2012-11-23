/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

using System;
using System.IO;
using System.Xml.Serialization;

namespace RpgCodeExpress.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class SerializableData
    {

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">File path.</param>
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

        #endregion

    }
}
