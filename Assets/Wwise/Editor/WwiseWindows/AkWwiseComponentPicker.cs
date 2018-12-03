#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

public class AkWwiseComponentPicker : UnityEditor.EditorWindow
{
	private static AkWwiseComponentPicker s_componentPicker;

	private readonly AkWwiseTreeView m_treeView = new AkWwiseTreeView();
	private bool m_close;
	private UnityEditor.SerializedProperty[] m_selectedItemGuid;
	private UnityEditor.SerializedProperty[] m_selectedItemID;
	private UnityEditor.SerializedObject m_serializedObject;
	private AkWwiseProjectData.WwiseObjectType m_type;

	/// <summary>
	///  The window to repaint after closing the picker
	/// </summary>
	public static UnityEditor.EditorWindow LastFocusedWindow = null;

	private void Update()
	{
		//Unity sometimes generates an error when the window is closed from the OnGUI function.
		//So We close it here
		if (m_close)
		{
			Close();

			if (LastFocusedWindow)
			{
				UnityEditor.EditorApplication.delayCall += LastFocusedWindow.Repaint;
				LastFocusedWindow = null;
			}
		}
	}

	private void OnGUI()
	{
		using (new UnityEngine.GUILayout.VerticalScope())
		{
			m_treeView.DisplayTreeView(AK.Wwise.TreeView.TreeViewControl.DisplayTypes.USE_SCROLL_VIEW);

			using (new UnityEngine.GUILayout.HorizontalScope("box"))
			{
				if (UnityEngine.GUILayout.Button("Ok"))
				{
					//Get the selected item
					var selectedItem = m_treeView.GetSelectedItem();

					//Check if the selected item has the correct type
					if (selectedItem != null && m_type == (selectedItem.DataContext as AkWwiseTreeView.AkTreeInfo).ObjectType)
						SetGuid(selectedItem);

					//The window can now be closed
					m_close = true;
				}
				else if (UnityEngine.GUILayout.Button("Cancel"))
					m_close = true;
				else if (UnityEngine.GUILayout.Button("Reset"))
				{
					ResetGuid();
					m_close = true;
				}
				else if (UnityEngine.Event.current.type == UnityEngine.EventType.Used && m_treeView.LastDoubleClickedItem != null &&
				         m_type == (m_treeView.LastDoubleClickedItem.DataContext as AkWwiseTreeView.AkTreeInfo).ObjectType)
				{
					//We must be in 'used' mode in order for this to work
					SetGuid(m_treeView.LastDoubleClickedItem);
					m_close = true;
				}
			}
		}
	}

	private void SetGuid(AK.Wwise.TreeView.TreeViewItem in_item)
	{
		m_serializedObject.Update();

		var obj = in_item.DataContext as AkWwiseTreeView.AkTreeInfo;
		//we set the items guid
		AkUtilities.SetByteArrayProperty(m_selectedItemGuid[0], obj.Guid);
		if (m_selectedItemID != null)
			m_selectedItemID[0].intValue = obj.ID;

		//When its a State or a Switch, we set the group's guid
		if (m_selectedItemGuid.Length == 2)
		{
			obj = in_item.Parent.DataContext as AkWwiseTreeView.AkTreeInfo;
			AkUtilities.SetByteArrayProperty(m_selectedItemGuid[1], obj.Guid);
			if (m_selectedItemID != null)
				m_selectedItemID[1].intValue = obj.ID;
		}

		m_serializedObject.ApplyModifiedProperties();
	}

	private void ResetGuid()
	{
		m_serializedObject.Update();

		var emptyArray = new byte[16];

		//we set the items guid
		AkUtilities.SetByteArrayProperty(m_selectedItemGuid[0], emptyArray);
		if (m_selectedItemID != null)
			m_selectedItemID[0].intValue = 0;

		//When its a State or a Switch, we set the group's guid
		if (m_selectedItemGuid.Length == 2)
		{
			AkUtilities.SetByteArrayProperty(m_selectedItemGuid[1], emptyArray);
			if (m_selectedItemID != null)
				m_selectedItemID[1].intValue = 0;
		}

		m_serializedObject.ApplyModifiedProperties();
	}

	public class PickerCreator
	{
		public UnityEditor.SerializedProperty[] guidProperty;
		public UnityEditor.SerializedProperty[] idProperty;
		public AkWwiseProjectData.WwiseObjectType objectType;
		public UnityEngine.Rect pickerPosition;
		public UnityEditor.SerializedObject serializedObject;

		internal PickerCreator()
		{
			UnityEditor.EditorApplication.delayCall += DelayCall;
		}

		private void DelayCall()
		{
			if (s_componentPicker != null)
				return;

			s_componentPicker = CreateInstance<AkWwiseComponentPicker>();

			//position the window below the button
			var pos = new UnityEngine.Rect(pickerPosition.x, pickerPosition.yMax, 0, 0);

			//If the window gets out of the screen, we place it on top of the button instead
			if (pickerPosition.yMax > UnityEngine.Screen.currentResolution.height / 2)
				pos.y = pickerPosition.y - UnityEngine.Screen.currentResolution.height / 2;

			//We show a drop down window which is automatically destroyed when focus is lost
			s_componentPicker.ShowAsDropDown(pos,
				new UnityEngine.Vector2(pickerPosition.width >= 250 ? pickerPosition.width : 250,
					UnityEngine.Screen.currentResolution.height / 2));

			s_componentPicker.m_selectedItemGuid = guidProperty;
			s_componentPicker.m_selectedItemID = idProperty;
			s_componentPicker.m_serializedObject = serializedObject;
			s_componentPicker.m_type = objectType;

			//Make a backup of the tree's expansion status and replace it with an empty list to make sure nothing will get expanded
			//when we populate the tree 
			var expandedItemsBackUp = AkWwiseProjectInfo.GetData().ExpandedItems;
			AkWwiseProjectInfo.GetData().ExpandedItems = new System.Collections.Generic.List<string>();

			s_componentPicker.m_treeView.AssignDefaults();
			s_componentPicker.m_treeView.SetRootItem(
				System.IO.Path.GetFileNameWithoutExtension(WwiseSetupWizard.Settings.WwiseProjectPath),
				AkWwiseProjectData.WwiseObjectType.PROJECT);

			//Populate the tree with the correct type 
			if (objectType == AkWwiseProjectData.WwiseObjectType.EVENT)
			{
				s_componentPicker.m_treeView.PopulateItem(s_componentPicker.m_treeView.RootItem, "Events",
					AkWwiseProjectInfo.GetData().EventWwu);
			}
			else if (objectType == AkWwiseProjectData.WwiseObjectType.SWITCH)
			{
				s_componentPicker.m_treeView.PopulateItem(s_componentPicker.m_treeView.RootItem, "Switches",
					AkWwiseProjectInfo.GetData().SwitchWwu);
			}
			else if (objectType == AkWwiseProjectData.WwiseObjectType.STATE)
			{
				s_componentPicker.m_treeView.PopulateItem(s_componentPicker.m_treeView.RootItem, "States",
					AkWwiseProjectInfo.GetData().StateWwu);
			}
			else if (objectType == AkWwiseProjectData.WwiseObjectType.SOUNDBANK)
			{
				s_componentPicker.m_treeView.PopulateItem(s_componentPicker.m_treeView.RootItem, "Banks",
					AkWwiseProjectInfo.GetData().BankWwu);
			}
			else if (objectType == AkWwiseProjectData.WwiseObjectType.AUXBUS)
			{
				s_componentPicker.m_treeView.PopulateItem(s_componentPicker.m_treeView.RootItem, "Auxiliary Busses",
					AkWwiseProjectInfo.GetData().AuxBusWwu);
			}
			else if (objectType == AkWwiseProjectData.WwiseObjectType.GAMEPARAMETER)
			{
				s_componentPicker.m_treeView.PopulateItem(s_componentPicker.m_treeView.RootItem, "Game Parameters",
					AkWwiseProjectInfo.GetData().RtpcWwu);
			}
			else if (objectType == AkWwiseProjectData.WwiseObjectType.TRIGGER)
			{
				s_componentPicker.m_treeView.PopulateItem(s_componentPicker.m_treeView.RootItem, "Triggers",
					AkWwiseProjectInfo.GetData().TriggerWwu);
			}
			else if (objectType == AkWwiseProjectData.WwiseObjectType.ACOUSTICTEXTURE)
			{
				s_componentPicker.m_treeView.PopulateItem(s_componentPicker.m_treeView.RootItem, "Virtual Acoustics",
					AkWwiseProjectInfo.GetData().AcousticTextureWwu);
			}

			AK.Wwise.TreeView.TreeViewItem item = null;

			var byteArray = AkUtilities.GetByteArrayProperty(guidProperty[0]);
			if (byteArray != null)
				item = s_componentPicker.m_treeView.GetItemByGuid(new System.Guid(byteArray));

			if (item != null)
			{
				item.ParentControl.SelectedItem = item;

				var itemIndexFromRoot = 0;

				//Expand all the parents of the selected item.
				//Count the number of items that are displayed before the selected item
				while (true)
				{
					item.IsExpanded = true;

					if (item.Parent != null)
					{
						itemIndexFromRoot += item.Parent.Items.IndexOf(item) + 1;
						item = item.Parent;
					}
					else
						break;
				}

				//Scroll down the window to make sure that the selected item is always visible when the window opens
				//there seems to be 1 pixel between each item so we add 2 pixels(top and bottom) 
				var itemHeight = item.ParentControl.m_skinSelected.button.CalcSize(new UnityEngine.GUIContent(item.Header)).y + 2.0f;
				s_componentPicker.m_treeView.SetScrollViewPosition(new UnityEngine.Vector2(0.0f,
					itemHeight * itemIndexFromRoot - UnityEngine.Screen.currentResolution.height / 4));
			}

			//Restore the tree's expansion status
			AkWwiseProjectInfo.GetData().ExpandedItems = expandedItemsBackUp;
		}
	}
}
#endif