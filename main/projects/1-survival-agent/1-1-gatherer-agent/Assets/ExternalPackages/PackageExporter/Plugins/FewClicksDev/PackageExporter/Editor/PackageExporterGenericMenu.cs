namespace FewClicksDev.PackageExporter
{
    using UnityEditor;
    using UnityEngine;

    public static class PackageExporterGenericMenu
    {
        private static readonly GUIContent SELECT_CONTENT = new GUIContent("Select", "Select the export setup in the project view.");
        private static readonly GUIContent DELETE_CONTENT = new GUIContent("Delete", "Delete the export setup.");
        private static readonly GUIContent SET_AS_DEFAULT_SETUP = new GUIContent("Set as default", "Set this setup as the one opened by default.");

        public static void ShowForSetup(PackageExporterWindow _window, Event _currentEvent, PackageExportSetup _preset)
        {
            GenericMenu _menu = new GenericMenu();

            _menu.AddDisabledItem(new GUIContent(_preset.DisplayName));
            _menu.AddSeparator(string.Empty);
            _menu.AddItem(SELECT_CONTENT, false, _selectAndPing);
            _menu.AddItem(DELETE_CONTENT, false, _delete);

            if (_preset.IsDefault == false)
            {
                _menu.AddSeparator(string.Empty);
                _menu.AddItem(SET_AS_DEFAULT_SETUP, false, _setAsDefault);
            }

            _menu.ShowAsContext();

            _currentEvent.Use();

            void _selectAndPing()
            {
                Selection.activeObject = _preset;
                EditorGUIUtility.PingObject(_preset);
            }

            void _delete()
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_preset));
                _window.RefreshSetups();
            }

            void _setAsDefault()
            {
                _preset.SetAsDefault();
            }
        }
    }
}
