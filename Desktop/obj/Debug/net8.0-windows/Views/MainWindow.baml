   M S B A M L   `   `   `  ����  Desktop    Desktop.Views.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)    � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%_  '��       �� 5   
   " ��6   .�5      ��  � �� �#05101c6+   �� 5	       � ��
 �����6+   �� 5
       � ��
 ���� 6,   �� 5       � ��$	 5,3q�6(   �� 5       � ��$	 5,0q�6'   �� 5       � ��$ 12(�6)   �� 5       �@�Cursor$
 Hand��6'   �� 5       � ��$ 5��6)   �� 5       � ��$ 5��6*   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 5��5   !   #��z��5   !   $��1q�5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5      5      .��5       � ��5!   
   �� 5"       �� ImageSource$( "/Resources/background_gradient.png��6   	 j� Stretch$	 UniformToFill=�6J   .+�5%   
   � $��50��6   $	��Top=�6   
������ $��0,20,0,0q�6L   
 � ColumnDefinitions
 5&      �� 5'      $��* �6#   �� 5(      $
��Auto �6#   .+�5+      �� -   5         ��Name$ CurrentPageTitle��5+      $��	SpeedTest��5,      
�������$��Center=�5.      $
��Left=�5/      $��16(�50      $��Segoe UI)�51      $��
10,-27,0,0q�52      �� 54       �� Orientation$ 
Horizontal=�6   $��Right=�62   $	��Top=�6N   $��88��6f   .+�55      �� -   5        .��56      �� -   5         ��Name$ CurrentImage��56      $��/Resources/settings.png��57      $��Uniform=�6=   $��25��6O   58      �� 5:      -   5        .��5;      �� -   5        $ ReloadImage��5;      $��/Resources/reload.png��5<      $��Uniform=�6;   $��25��6M   5=      5>      5?       � 5A   
   -   5          �Name$ 	MainFrame��5A        � NavigationUIVisibility$ Hidden=�6#   $��0,50,0,0q�6C   6U   5B      5C      