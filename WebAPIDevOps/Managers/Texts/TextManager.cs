using System.Collections.Generic;
using System.Linq;
using WebApiDevOps.Server.Initialization;
using WebApiDevOps.Server.Managers;
using WebAPIDevOps.Constant.Enums;
using WebAPIDevOps.Server.Databases.Texts;

namespace WebAPIDevOps.Server.Managers.Texts
{
    public class TextManager : DataManager<TextManager>
    {
        Dictionary<int, TextRecord> m_texts = new Dictionary<int, TextRecord>();
        [Initialization(InitializationPass.First)]
        public override void Initialize()
        {
            m_texts = Database.Query<TextRecord>(TextRecordRelator.FetchQueries).ToDictionary(x => x.Id);
        }

        public string GetText(TextIdEnum textId, LangIdEnum langId)
        {
            if (m_texts.ContainsKey((int)textId))
            {
                switch (langId)
                {
                    case LangIdEnum.French:
                        return m_texts[(int)textId].French;
                    case LangIdEnum.English:
                        return m_texts[(int)textId].English;
                    case LangIdEnum.Spanish:
                        return m_texts[(int)textId].Spanish;
                    case LangIdEnum.Italian:
                        return m_texts[(int)textId].Italian;
                    case LangIdEnum.German:
                        return m_texts[(int)textId].German;
                }
            }
            return "(Text not found)";
        }
    }
}