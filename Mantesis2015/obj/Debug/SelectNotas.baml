   M S B A M L   `   `   `  ����  Mantesis2015    Mantesis2015.SelectNotas   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      k gTelerik.Windows.Documents.Core, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p	 lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7z vTelerik.Windows.Controls.FixedDocumentViewers, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Otelerik1http://schemas.telerik.com/2008/xaml/presentation	    	 
     5      -   5        $	��600��5      $	��500��5      $"��Seleccionar nota aclaratoria��5      )  #Telerik.Windows.Controls.WindowBase    
ResizeMode$  NoResize=�5	      (  "Telerik.Windows.Controls.RadWindow   WindowStartupLocation$ CenterScreen=�5
      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   �� $��	30,20,0,0q�6   $
��Left=�5      $	��Top=�5      $��Selecccionar nota:��5      *  $Telerik.Windows.Controls.RadComboBox  5   
   -   5          Name$ CbxNotas��5      $��30,45,30,10q�5      $��Stretch=�5      $��Stretch=�5       �� DisplayMemberPath$
 Nota��5      ��5      �� 5      �� 5   
   $��	30,10,0,0q�6   $��1��6#   $
��Left=�5      $	��Top=�5      $��Descripción completa:��5       �� 5!   
   $��30q�6   $��1��6#   .��5"   
   �� -   5         ��Name$ TxtDescripcion��5"      $��30q�5#      $��1��6    $
��Wrap=�5%      ��5$      �� 5%      5&      �� 5'   
   -   5         ��Name$ BtnCancelar��5'      $��2��6&   $��75��5(      $��28��5)      $��	0,10,30,0q�5*      $��Right=�5+      $	��Top=�5,      $��Cancelar��5.       �� IsCancel . 5/      �� 50   
   -   5        $ 
BtnAceptar��50      $��2��6%   $��75��51      $��28��52      $��
0,10,110,0q�53      $��Right=�54      $	��Top=�55      $��Aceptar��57       �� 	IsDefault . 58      5:      5;      