using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Localization;
#endif

namespace nobodyworks.builder.extensions
{
    public static class LocalizedStringExtensions
    {
        public static string GetText(this LocalizedString localizedString)
        {
            if (localizedString == null)
            {
                return string.Empty;
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (!localizedString.IsEmpty)
                {
                    var tableCollection =
                        LocalizationEditorSettings.GetStringTableCollection(localizedString.TableReference);
                    var locale = Editor_GetValidLocaleInEditMode(tableCollection);

                    if (locale != null)
                    {
                        var table = (StringTable)tableCollection.GetTable(locale.Identifier);

                        if (table != null)
                        {
                            return table.GetEntryFromReference(localizedString.TableEntryReference)?.LocalizedValue;
                        }
                    }
                }

                return string.Empty;
            }
            
            if (localizedString.IsEmpty)
            {
                return string.Empty;
            }
#endif

            return localizedString.GetLocalizedString();
        }
        
#if UNITY_EDITOR
        static Locale Editor_GetValidLocaleInEditMode(LocalizationTableCollection tableCollection)
        {
            foreach (var locale in LocalizationEditorSettings.GetLocales())
            {
                if (locale != null && (tableCollection == null
                                       || tableCollection.GetTable(locale.Identifier) != null))
                {
                    return locale;
                }
            }

            return null;
        }
#endif
    }
}