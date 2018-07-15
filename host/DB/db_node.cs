using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using System.IO;
using System.Data;
using System.Xml;
using Newtonsoft.Json;
using System.Collections;
using System.Xml.Linq;
using System.Reflection;

using model;
using System.Linq.Dynamic;
using System.Runtime.Caching;

namespace host
{
    public class db_node
    {
        private static int m_type_device = 99;
        private static string
            path_node = hostServer.pathRoot + @"db_node\",
            path_ID_parent = hostServer.pathRoot + @"db_node_parent\",
            path_ID_node = hostServer.pathRoot + @"db_node_sub\",
            path_ID_device_all = hostServer.pathRoot + @"db_node_device\",
            path_ID_device_near = hostServer.pathRoot + @"db_node_device_near\";

        private static object lock_node = new object();
        private static object lock_ID_node = new object();

        private static object lock_ID_device_all = new object();
        private static object lock_ID_device_near = new object();

        private static Dictionary<long, m_node> dic_node = new Dictionary<long, m_node>() { };
        private static Dictionary<string, long> dic_node_code = new Dictionary<string, long>() { };
        private static DictionaryList<long, long> dic_ID_node = new DictionaryList<long, long>() { }; //<node_id, child_ids>
        private static DictionaryList<long, long> dic_ID_device_all = new DictionaryList<long, long>() { }; //lưu tổng điểm đo của 1 node (bao gồm tổng của các node con)
        private static DictionaryList<long, long> dic_ID_device_near = new DictionaryList<long, long>() { }; //lưu các điểm của 1 node được chọn gần điểm đo nhát

        private static object lock_ID_parent = new object();
        private static DictionaryList<long, long> dic_ID_parent = new DictionaryList<long, long>() { }; //<node_id, parent_ids>

        private static object lock_cache = new object();
        private static Dictionary<long, string> dic_ID_cache = new Dictionary<long, string>() { };

        #region /// update, load ...

        public static void update_node(long id, m_node o)
        {
            hostFile.write_MMF<m_node>(o, path_node, id.ToString());
            //dbCache.clear(typeof(m_node).FullName);
        }

        public static void update_ID_parent(long id, List<long> ids)
        {
            ids.Insert(0, id);
            hostFile.write_file_MMF<long>(ids, path_ID_parent, id.ToString());
            ids.RemoveAt(0);
        }

        public static void update_ID_node(long id, List<long> ids)
        {
            ids.Insert(0, id);
            hostFile.write_file_MMF<long>(ids, path_ID_node, id.ToString());
            ids.RemoveAt(0);
        }

        public static void update_ID_device_all(long id, List<long> ids)
        {
            ids.Insert(0, id);
            hostFile.write_file_MMF<long>(ids, path_ID_device_all, id.ToString());
            ids.RemoveAt(0);
        }

        public static void update_ID_device_near(long id, List<long> ids)
        {
            ids.Insert(0, id);
            hostFile.write_file_MMF<long>(ids, path_ID_device_near, id.ToString());
            ids.RemoveAt(0);
        }

        public static void load()
        {
            if (!Directory.Exists(path_node))
                Directory.CreateDirectory(path_node);
            else
            {
                var ds = Directory.GetFiles(path_node, "*.mmf");
                for (int k = 0; k < ds.Length; k++)
                {
                    m_node o = hostFile.read_MMF<m_node>(ds[k]);
                    if (o.id != 0)
                    {
                        if (dic_node.ContainsKey(o.id) == false)
                            dic_node.Add(o.id, o);
                        if (dic_node_code.ContainsKey(o.code) == false)
                            dic_node_code.Add(o.code, o.id);
                    }
                }
            }

            if (!Directory.Exists(path_ID_parent))
                Directory.CreateDirectory(path_ID_parent);
            else
            {
                var ds = Directory.GetFiles(path_ID_parent, "*.mmf");
                for (int k = 0; k < ds.Length; k++)
                {
                    List<long> ls = hostFile.read_file_MMF<long>(ds[k]).Distinct().ToList();
                    if (ls.Count > 0)
                    {
                        long id = ls[0];
                        ls = ls.Where(x => x != 0 && x != id).ToList();
                        if (dic_ID_parent.ContainsKey(id) == false)
                            dic_ID_parent.AddListDistinct(id, ls.ToArray());
                    }
                }
            }

            if (!Directory.Exists(path_ID_node))
                Directory.CreateDirectory(path_ID_node);
            else
            {
                var ds = Directory.GetFiles(path_ID_node, "*.mmf");
                for (int k = 0; k < ds.Length; k++)
                {
                    List<long> ls = hostFile.read_file_MMF<long>(ds[k]).Distinct().ToList();
                    if (ls.Count > 0)
                    {
                        long id = ls[0];
                        ls = ls.Where(x => x != 0 && x != id).ToList();
                        if (dic_ID_node.ContainsKey(id) == false)
                            dic_ID_node.AddListDistinct(id, ls.ToArray());
                    }
                }
            }


            if (!Directory.Exists(path_ID_device_all))
                Directory.CreateDirectory(path_ID_device_all);
            else
            {
                var ds = Directory.GetFiles(path_ID_device_all, "*.mmf");
                for (int k = 0; k < ds.Length; k++)
                {
                    List<long> ls = hostFile.read_file_MMF<long>(ds[k]).Distinct().ToList();
                    if (ls.Count > 0)
                    {
                        long id = ls[0];
                        ls = ls.Where(x => x != 0 && x != id).ToList();

                        if (dic_ID_device_all.ContainsKey(id) == false)
                            dic_ID_device_all.AddListDistinct(id, ls.ToArray());
                    }
                }
            }


            if (!Directory.Exists(path_ID_device_near))
                Directory.CreateDirectory(path_ID_device_near);
            else
            {
                var ds = Directory.GetFiles(path_ID_device_near, "*.mmf");
                for (int k = 0; k < ds.Length; k++)
                {
                    List<long> ls = hostFile.read_file_MMF<long>(ds[k]).Distinct().ToList();
                    if (ls.Count > 0)
                    {
                        long id = ls[0];
                        ls = ls.Where(x => x != 0 && x != id).ToList();

                        if (dic_ID_device_near.ContainsKey(id) == false)
                            dic_ID_device_near.AddListDistinct(id, ls.ToArray());
                    }
                }
            }

            cache_node_id(1);
        }

        #endregion

        #region /// Add, Edit, Remove ...

        public static Tuple<bool, string, dynamic> add(string s_key, string s_item)
        {
            try
            {
                string json = "";

                var o = JsonConvert.DeserializeObject<m_node>(s_item);
                m_node m = JsonConvert.DeserializeObject<m_node>(s_item);

                if (dic_node_code.ContainsKey(m.code) == false)
                {
                    long id_new = 0;
                    bool c = true;
                    while (c == true)
                    {
                        id_new = DateTime.Now.ToString("yyyyMMddHHmmssfff").TryParseToLong() + new Random().Next(10, 99);
                        c = dic_node.ContainsKey(id_new);
                    }

                    int phase_type = o.id_group;
                    long id_parent = o.id_parent;

                    m.type = o.type;

                    m.id = id_new;
                    m.id_group = phase_type;
                    m.id_parent = id_parent;

                    m.id_root0 = o.id_root0;
                    m.id_root1 = o.id_root1;

                    m.level = o.level;
                    m.name = o.name;
                    m.code = o.code;

                    lock (lock_node)
                    {
                        dic_node_code.Add(m.code, id_new);
                        dic_node.Add(id_new, m);
                        cache_node_id(phase_type);
                    }
                    update_node(id_new, m);

                    lock (lock_ID_node)
                        dic_ID_node.AddDistinct(id_parent, id_new);

                    List<long> ls = new List<long>() { };
                    if (dic_ID_node.TryGetValue(id_parent, out ls) == false) ls = new List<long>() { };
                    update_ID_node(id_parent, ls);

                    Task.Factory.StartNew(() => { cache_node_set(phase_type, id_parent); });

                    json = JsonConvert.SerializeObject(m);

                    List<long> ls_parent = new List<long>() { };
                    if (dic_ID_parent.TryGetValue(id_parent, out ls_parent) == false) ls_parent = new List<long>() { };
                    ls_parent.Add(id_parent);
                    lock (lock_ID_parent)
                        dic_ID_parent.AddListDistinct(m.id, ls_parent);
                    update_ID_parent(m.id, ls_parent);
                }
                else
                {
                    json = "trung_ma";
                }

                return new Tuple<bool, string, dynamic>(true, json, m);
            }
            catch { }

            return new Tuple<bool, string, dynamic>(false, "", null);
        }

        public static Tuple<bool, string, dynamic> edit(string s_key, string s_item)
        {
            try
            {
                string json = "";

                m_node m = JsonConvert.DeserializeObject<m_node>(s_item);

                if (dic_node.ContainsKey(m.id) == true && m.id != 0)
                {
                    m_node o = new m_node();
                    dic_node.TryGetValue(m.id, out o);

                    if (o.name != m.name || o.type != m.type)
                    {
                        o.name = m.name;
                        o.type = m.type;
                        if (dic_node.ContainsKey(o.id))
                        {
                            lock (lock_node)
                                dic_node[o.id] = o;

                            update_node(o.id, o);
                            Task.Factory.StartNew(() => { cache_node_set(o.id_group, o.id_parent); });
                        }
                    }
                    json = JsonConvert.SerializeObject(o);
                }

                return new Tuple<bool, string, dynamic>(true, json, m);
            }
            catch { }
            return new Tuple<bool, string, dynamic>(false, "", null);
        }

        public static Tuple<bool, string, dynamic> remove(string s_key, string s_item)
        {
            try
            {
                string json = "";
                var id_ = s_item.Split('|');

                if (id_.Length > 0)
                {
                    m_node m = new m_node();
                    if (dic_node.TryGetValue(long.Parse(id_[0]), out m) == true)
                    {
                        json = JsonConvert.SerializeObject(m);

                        lock (lock_ID_device_near)
                        {
                            if (dic_ID_device_near.ContainsKey(long.Parse(id_[0])) == true)
                            {
                                if (dic_ID_device_near.ContainsKey(long.Parse(id_[0]))) dic_ID_device_near.Remove(long.Parse(id_[0]));
                                string f = path_ID_device_near + @"\" + id_.ToString() + ".mmf";
                                f = f.Replace(@"\\", @"\");
                                if (File.Exists(f)) File.Delete(f);
                            }
                        }

                        lock (lock_ID_device_all)
                        {
                            if (dic_ID_device_all.ContainsKey(long.Parse(id_[0])) == true)
                            {
                                if (dic_ID_device_all.ContainsKey(long.Parse(id_[0]))) dic_ID_device_all.Remove(long.Parse(id_[0]));
                                string f = path_ID_device_all + @"\" + id_.ToString() + ".mmf";
                                f = f.Replace(@"\\", @"\");
                                if (File.Exists(f)) File.Delete(f);
                            }
                        }

                        lock (lock_ID_node)
                        {
                            List<long> ls = new List<long>() { };
                            dic_ID_node.TryGetValue(m.id_parent, out ls);
                            if (ls.IndexOf(long.Parse(id_[0])) != -1)
                            {
                                ls.Remove(long.Parse(id_[0]));
                                update_ID_node(m.id_parent, ls);
                            }
                        }

                        lock (lock_node)
                        {
                            if (dic_node_code.ContainsKey(m.code)) dic_node_code.Remove(m.code);
                            if (dic_node.ContainsKey(long.Parse(id_[0]))) dic_node.Remove(long.Parse(id_[0]));
                            cache_node_id(int.Parse(id_[1]));
                        }
                        string fo = path_node + @"\" + id_.ToString() + ".mmf";
                        fo = fo.Replace(@"\\", @"\");
                        if (File.Exists(fo)) File.Delete(fo);

                        lock (lock_ID_parent)
                        {
                            if (dic_ID_device_all.ContainsKey(long.Parse(id_[0])) == true)
                            {
                                if (dic_ID_parent.ContainsKey(long.Parse(id_[0]))) dic_ID_parent.Remove(long.Parse(id_[0]));

                                string f = path_ID_device_all + @"\" + id_.ToString() + ".mmf";
                                f = f.Replace(@"\\", @"\");
                                if (File.Exists(f)) File.Delete(f);
                            }

                            var ls_parent = dic_ID_parent.Where(x => x.Value.IndexOf(long.Parse(id_[0])) != -1).Select(x => x).ToList();
                            foreach (var ke in ls_parent)
                            {
                                List<long> ls = ke.Value;
                                ls.Remove(long.Parse(id_[0]));
                                dic_ID_parent[ke.Key] = ls;
                                update_ID_parent(ke.Key, ls);
                                cache_node_set(int.Parse(id_[1]), ke.Key);
                            }

                        }

                        cache_node_id(int.Parse(id_[1]));
                    }
                }
                return new Tuple<bool, string, dynamic>(true, json, null);
            }
            catch { }
            return new Tuple<bool, string, dynamic>(false, "", null);
        }

        #endregion

        #region /// cache ...

        //groupid = 0: all, 1: 1 pha, 33: 3 pha, 31: 3 pha 1 bieu
        private static void cache_node_id(int groupid)
        {

            string data = string.Join("|",
                              dic_node.Values.Where(x => x.id_group > 0).Select(x => x.id.ToString() + "." + x.id_group.ToString()).ToArray());

            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.Priority = System.Runtime.Caching.CacheItemPriority.Default;
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddDays(30);
            //Muốn tạo sự kiện thì tạo ở đây còn k thì hoy
            //(MyCacheItemPriority == MyCachePriority.Default) ? CacheItemPriority.Default : CacheItemPriority.NotRemovable;
            ///Globals.policy.RemovedCallback = callback;             
            cache.Set("node", data, policy, null);
        }

        public static string cache_node_get(int group, long id)
        {
            return cache_node_set(group, id);

            //ObjectCache cache = MemoryCache.Default;
            //string data = cache["node." + idnode.ToString()] as string;
            //if (data == null)
            //{
            //    return cache_node_set(idnode);
            //}
            //else
            //    return data;
        }

        public static string cache_node_set(int phase_id, long id_node)
        {
            List<long> ids_node = new List<long>() { };
            List<long> ids_device = new List<long>() { };
            List<long> ids_device_near = new List<long>() { };

            string data = "", key = "node." + phase_id.ToString() + "|" + id_node.ToString(CultureInfo.InvariantCulture);
            //ids_node = dic_ID_node.ToList().First(x => x.Key.Equals(idnode)).Value;
            //ids_device = dic_ID_device.ToList().First(x => x.Key.Equals(idnode)).Value;

            dic_ID_node.TryGetValue(id_node, out ids_node);
            dic_ID_device_all.TryGetValue(id_node, out ids_device);
            dic_ID_device_near.TryGetValue(id_node, out ids_device_near);
            if (ids_device_near == null) ids_device_near = new List<long>() { };

            if (dic_ID_node.TryGetValue(id_node, out ids_node) || dic_ID_device_all.TryGetValue(id_node, out ids_device))
            //if ((ids_device != null || (ids_node != null) && (ids_node.Count > 0 || ids_device.Count > 0)))
            {
                if (ids_node == null) ids_node = new List<long>() { };
                if (ids_device == null) ids_device = new List<long>() { };
                if (ids_node.Count > 0 || ids_device.Count > 0)
                {
                    ids_node = ids_node.Where(x => x != id_node).ToList();
                    ids_node = ids_node.Where(
                        o => dic_node.Where(x => x.Value.id_group == phase_id).
                            Select(y => y.Value.id).Contains(o)).ToList();
                    ids_device = ids_device.Where(x => x != id_node).ToList();
                    var existList = dic_ID_device_all.Where(x => x.Key == id_node).ToList();
                    ids_device = ids_device.Where(x => existList.Any(o => o.Value.Contains(x))).ToList();

                    if (ids_node.Count > 0)
                    {
                        for (int k = 0; k < ids_node.Count; k++)
                        {
                            long id_ = ids_node[k];
                            m_node o = new m_node();
                            if (dic_node.TryGetValue(id_, out o))
                            {
                                string len_sub_node = "|0";
                                List<long> ls_sub_node = new List<long>() { };
                                if (dic_ID_node.TryGetValue(id_, out ls_sub_node))
                                    len_sub_node = "|" + ls_sub_node.Where(x => x != id_).Distinct().Count().ToString();

                                string len_sub_device = "|0";
                                List<long> ls_sub_device = new List<long>() { };
                                if (dic_ID_device_all.TryGetValue(id_, out ls_sub_device))
                                {
                                    int leni = ls_sub_device.Where(x => x != 0 && x != id_).Distinct().Count();
                                    len_sub_device = "|" + leni.ToString();
                                }

                                if (data == "")
                                    data = phase_id.ToString() + "|" + o.type.ToString() + "|1|" + o.id + "|" + o.name + len_sub_node + len_sub_device + Environment.NewLine;
                                else
                                    data += "@" + phase_id.ToString() + "|" + o.type.ToString() + "|1|" + o.id + "|" + o.name + len_sub_node + len_sub_device + Environment.NewLine;
                            }
                        }
                    }

                    if (ids_device_near.Count > 0)
                    {
                        //m_node node = new m_node();
                        //if (dic_node.TryGetValue(id_node, out node))
                        //{
                        //    if (node.type != 0 && node.type != 99)
                        //    {

                        var ls = db_meter.get_Items(ids_device_near.ToArray());
                        for (int k = 0; k < ls.Length; k++)
                        {
                            var o = ls[k];
                            if (data == "")
                                data = phase_id.ToString() + "|" + m_type_device.ToString() + "|1|" + o.meter_id +
                                       "|" + o.name + "|0|0" + Environment.NewLine;
                            else
                                data += "@" + phase_id.ToString() + "|" + m_type_device.ToString() + "|1|" +
                                        o.meter_id + "|" + o.name + "|0|0" + Environment.NewLine;
                        }

                        // }
                        //}
                    }
                }

                ObjectCache cache = MemoryCache.Default;
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.Priority = System.Runtime.Caching.CacheItemPriority.Default;
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddDays(3);
                //Muốn tạo sự kiện thì tạo ở đây còn k thì hoy
                //(MyCacheItemPriority == MyCachePriority.Default) ? CacheItemPriority.Default : CacheItemPriority.NotRemovable;
                ///Globals.policy.RemovedCallback = callback;             
                cache.Set(key, data, policy, null);
            }

            return data;
        }

        #endregion

        #region // where, where_index ...

        public static Tuple<bool, string, dynamic[]> get_id(string s_key)
        {
            long id = s_key.TryParseToLong();
            m_node o = new m_node();
            if (dic_node.TryGetValue(id, out o))
            {
                m_node p = new m_node();
                if (o.id_parent != 0 && dic_node.TryGetValue(o.id_parent, out p))
                    return new Tuple<bool, string, dynamic[]>(true, "", new dynamic[] { o, p });
                else
                    return new Tuple<bool, string, dynamic[]>(true, "", new dynamic[] { o });
            }

            return new Tuple<bool, string, dynamic[]>(false, "", null);
        }

        public static Tuple<bool, string, int, int, IList> where_call_dynamic(string s_key, string s_select, string s_where, string s_order_by, string s_distinct, int page_number, int page_size)
        {
            return dbQuery.where<m_node>(dic_node.Values.ToArray(), s_key, s_select, s_where, s_order_by, s_distinct, page_number, page_size);
        }

        public static Tuple<bool, string, int, int, IList> where_index_call_dynamic(string s_index, string s_select, string s_where, string s_order_by, string s_distinct, int page_number, int page_size)
        {
            int rs_total = dic_node.Count,
                rs_count = 0;

            try
            {
                long[] a_index = new long[] { };
                if (!string.IsNullOrEmpty(s_index))
                    a_index = s_index.Split(';').Select(x => x.TryParseToLong()).Where(x => x > 0).ToArray();

                List<long> ls = new List<long>() { };
                if (a_index.Length > 0)
                {
                    for (int k = 0; k < a_index.Length; k++)
                    {
                        long key = a_index[k];
                        List<long> li = new List<long>() { };
                        if (dic_ID_device_all.TryGetValue(key, out li) == false) li = new List<long>() { };
                        if (li != null && li.Count > 0)
                            ls.AddRange(li);
                    }
                }

                var ds = db_meter.get_Items(ls.ToArray());
                rs_count = ds.Length;

                if (rs_count > 0)
                    return dbQuery.where<m_meter>(ds, s_index, s_select, s_where, s_order_by, s_distinct, page_number, page_size);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, int, int, IList>(true, ex.Message, rs_total, 0, null);
            }

            return new Tuple<bool, string, int, int, IList>(true, "", rs_total, 0, null);
        }

        #endregion

        #region // join device, unjoin device to Node ...

        public static long[] get_IDs_node(long id_parent)
        {
            List<long> ls = new List<long>() { };
            if (dic_ID_node.TryGetValue(id_parent, out ls) == false) ls = new List<long>() { };
            return ls.ToArray();
        }

        public static long[] join_device_get(long id_key)
        {
            List<long> ls = new List<long>() { };
            if (dic_ID_device_all.TryGetValue(id_key, out ls) == false) ls = new List<long>() { };
            return ls.ToArray();
        }

        public static long[] get_IDs_device(long id_parent)
        {
            List<long> ls = new List<long>() { };
            if (dic_ID_device_all.TryGetValue(id_parent, out ls) == false) ls = new List<long>() { };
            return ls.ToArray();
        }

        public static int join_device_add(int groupid, long id_parent, long id_key, long[] vals)
        {
            vals = vals.Where(x => x > 1000000).ToArray();
            List<long> ls = new List<long>() { };
            if (vals.Length > 0)
            {
                List<long> ls0 = new List<long>() { };
                lock (lock_ID_device_near)
                    ls0 = dic_ID_device_near.AddListDistinctItem(id_key, vals);
                update_ID_device_near(id_key, ls0);

                lock (lock_ID_device_all)
                    ls = dic_ID_device_all.AddListDistinctItem(id_key, vals);
                update_ID_device_all(id_key, ls);
                cache_node_set(groupid, id_key);

                List<long> ls_parent = new List<long>() { };
                var dp = dic_ID_parent
                    .Where(x => x.Value.IndexOf(id_parent) != -1)
                    .Select(x => x.Value.ToArray()).ToArray();
                foreach (var ids in dp)
                    ls_parent.AddRange(ids);
                ls_parent = ls_parent.Where(x => x != id_key).Distinct().ToArray().Reverse().ToList();
                foreach (long id in ls_parent)
                {
                    if (id > 0)
                    {
                        List<long> lsi = new List<long>() { };

                        lock (lock_ID_device_all)
                            lsi = dic_ID_device_all.AddListDistinctItem(id, ls.ToArray());
                        lsi = lsi.Where(x => x > 1000000).ToList();

                        update_ID_device_all(id, lsi);
                    }
                }

                foreach (long id in ls_parent) cache_node_set(groupid, id);
                dbCache.clear(typeof(m_meter).FullName);
            }

            return ls.Count;
        }


        public static int join_device_remove(int groupid, long id_parent, long id_key, long[] vals)
        {
            int k = 0;

            List<long> ls0 = new List<long>() { };
            if (dic_ID_device_near.TryGetValue(id_key, out ls0)) {                 
                var lso = ls0.Where(x => !vals.Any(o => o == x)).ToList();
                if (lso.Count != vals.Length)
                {
                    lock (lock_ID_device_near)
                        dic_ID_device_near[id_key] = lso;
                    update_ID_device_near(id_key, lso);                
                }
            }

            List<long> ls = new List<long>() { };
            if (dic_ID_device_all.TryGetValue(id_key, out ls))
            {
                var lso = ls.Where(x => !vals.Any(o => o == x)).ToList();
                if (lso.Count != vals.Length)
                {
                    lock (lock_ID_device_all)
                        dic_ID_device_all[id_key] = lso;
                    update_ID_device_all(id_key, lso);
                    cache_node_set(groupid, id_key);

                    List<long> ls_parent = new List<long>() { };
                    var dp = dic_ID_parent
                        .Where(x => x.Value.IndexOf(id_parent) != -1)
                        .Select(x => x.Value.ToArray()).ToArray();
                    foreach (var ids in dp)
                        ls_parent.AddRange(ids);
                    ls_parent = ls_parent.Where(x => x != 0 && x != id_key).Distinct().ToList();
                    foreach (long id in ls_parent)
                    {
                        List<long> lsi = new List<long>() { };
                        if (dic_ID_device_all.TryGetValue(id, out lsi))
                        {
                            var lxi = lsi.Where(x => !vals.Any(o => o == x)).ToList();
                            lock (lock_ID_device_all) dic_ID_device_all[id] = lxi;

                            update_ID_device_all(id, lxi);
                            cache_node_set(groupid, id);
                        }
                    }
                    dbCache.clear(typeof(m_meter).FullName);
                }
            }

            return k;
        }

        public static int device_insert_remove(long[] meter_add, long[] meter_remove)
        {
            var parentOfMeter = dic_ID_device_all.Where(x => x.Value.IndexOf(meter_remove[0]) != -1).Select(x => x.Key).ToArray();
            var parentOfNodes =
                dic_ID_parent.Where(x => parentOfMeter.Any(o => o == x.Key)).Select(x => x.Key).ToArray();
            var listNeedUpdate = dic_ID_device_all.Where(x => parentOfNodes.Any(o => o.Equals(x.Key))).ToList();

            dic_ID_device_all.Where(x => listNeedUpdate.Any(r => r.Key.Equals(x.Key)))
                .ForEach(key => key.Value.Remove(meter_remove[0]));
            dic_ID_device_all.Where(x => listNeedUpdate.Any(r => r.Key.Equals(x.Key)))
                .ForEach(key => key.Value.Remove(meter_add[0]));

            return 2;
        }

        #endregion
    }
}
