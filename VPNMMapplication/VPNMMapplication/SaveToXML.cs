using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace VPNMMapplication
{
    public class SaveToXML
    {
        public static void BuildXmlDoc(MM_MK_Collection collection, string pathToSave)
        {
            XElement doc =
                new XElement("address_book", new XAttribute("version", "63006"),
                    new XElement("groups"),
                    new XElement("connections", collection.TheCollection.Select(unit =>
                        new XElement("connection", new XAttribute("parent_group_id", ""),
                            new XElement("InternalID"),
                            new XElement("Caption", unit.Title+" "+unit.MainOrReserve),
                            new XElement("PeerIP"),
                            new XElement("PeerHost", unit.DNS_Name),
                            new XElement("Port", "5650"),
                            new XElement("RenderMode", "1"),
                            new XElement("CPUUsage", "1"),
                            new XElement("ColorDepth", "5"),
                            new XElement("AdvMouseSroll", "true"),
                            new XElement("ConstProportions", "true"),
                            new XElement("SavePass", "false"),
                            new XElement("AutoLogin", "false"),
                            new XElement("Flags", "0"),
                            new XElement("ChacheMode", "0"),
                            new XElement("LocalCursorMode", "0"),
                            new XElement("RemoteCursorMode", "0"),
                            new XElement("UseCascadeConnect", "false"),
                            new XElement("CascadeHost"),
                            new XElement("LoginList"),
                            new XElement("DomainList"),
                            new XElement("blank_screen_text"),
                            new XElement("fps", "15"),
                            new XElement("allow_callback", "false"),
                            new XElement("mac"),
                            new XElement("lock_on_disconnect", "false"),
                            new XElement("display_index", "-1"),
                            new XElement("disable_dnd", "false"),
                            new XElement("use_ping", "false"),
                            new XElement("ping_interval", "15"),
                            new XElement("comp_hash"),
                            new XElement("ftp_last_work_dir"),
                            new XElement("black_screen_id"),
                            new XElement("use_ip_v_6", "false"),
                            new XElement("need_authority_server", "false"),
                            new XElement("authority_server_code"),
                            new XElement("ad_guid"),
                            new XElement("pwd_new"),
                            new XElement("Pwd"),
                            new XElement("need_my_user_name", "false"),
                            new XElement("my_user_name"),
                            new XElement("use_internet_id", "false"),
                            new XElement("peer_id"),
                            new XElement("use_custom_inet_server", "false"),
                            new XElement("inet_server"),
                            new XElement("inet_id_port", "5655"),
                            new XElement("use_inet_id_ip_v_6", "false"),
                            new XElement("inet_id_use_old_protocol", "false"),
                            new XElement("proxy_settings"),
                            new XElement("window_width", "0"),
                            new XElement("window_height", "0"),
                            new XElement("window_left", "0"),
                            new XElement("window_top", "0"),
                            new XElement("rdp_file_data"),
                            new XElement("rdp_use_direct_connection", "false"),
                            new XElement("rdp_use_external_client", "false"),
                            new XElement("rdp_user"),
                            new XElement("rdp_password"),
                            new XElement("rdp_save_password", "false"),
                            new XElement("rdp_screen_width", "800"),
                            new XElement("rdp_screen_height", "600"),
                            new XElement("rdp_color_mode", "16"),
                            new XElement("rdp_full_screen", "false"),
                            new XElement("rdp_themes", "false"),
                            new XElement("rdp_allow_animation", "false"),
                            new XElement("rdp_bitmap_caching", "false"),
                            new XElement("rdp_desktop_background", "false"),
                            new XElement("rdp_show_window_content", "false"),
                            new XElement("rdp_port", "3389"),
                            new XElement("rdp_stretch", "false"),
                            new XElement("av_audio_device_guid"),
                            new XElement("av_video_device_guid"),
                            new XElement("av_quality_av", "0"),
                            new XElement("av_capture_mode", "0"),
                            new XElement("av_sync_freq", "0"),
                            new XElement("rem_reg_last_key"),
                            new XElement("sound_capture", "false"),
                            new XElement("auto_screen_recording", "false"),
                            new XElement("comment"),
                            new XElement("sort_order", "0"),
                            new XElement("av_chat_local_settings"),
                            new XElement("av_chat_remote_settings")
                            )
            ))
            );
            // Save to file.
            doc.Save($"{pathToSave}.xml");
        }
    }
}

