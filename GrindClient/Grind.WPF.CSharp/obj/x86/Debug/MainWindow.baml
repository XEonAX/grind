   M S B A M L   `   `   `  ����K  GGrind.WPF.CSharp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Grind.WPF.CSharpGrind.WPF.CSharp  5   	   !    Grind.WPF.CSharp.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Grind.WPF.CSharp     5   	   -   5          � Title$  
MainWindow��5   	   $	��Top=�6   $	��553��64   $	��757��6A   $
��Left=�6M   .��5
      �  � RowDefinitions 5   
   �� 5      �� 5      $��20 �6   .+�5   
   �� $��20��6   $��Bottom=�6    $��1��6;   .@�5      �� -   5         ��Name$ 
sbiMessage��5      $��Message��60   $
��Left=�6B    � ToolTip 6]   ��  �� ElementName$ 
sbiMessage�� �� Path$ Content��� 5      -   5        $ sbiState��5      $��State��6.   $��Right=�6>    6Z   �� $ sbiState��$ Content�5      �� 5   
   .@�5      �� -   5         ��Name$ tbTasks��5   %   $��Tasks��6   .��5      �� $��1q�6   ���	#FF688BAE._�5      � -   5         �Name$ MainGrid��5       5      �� 5      $��28 �6,   �� 5      ��6,   �� *ActualHeight+$ MainGrid���� 5      $��6 �6,   �� 5      �� 5      $��20 �6,   .+�5      �� $��0��6%   $��Center=�62    �� Orientation$ 
Horizontal=�6M   $��2,0,0,0q�6f   .+�5       �� -   5        	 ��Name$	 
btnRefresh��5    E   $��Refresh��6&   $	��2,0q�68   6Y   �� 5!      -   5        $	 btnDownloadTasks��5!   U   $��Get and Store All Tasks��6&   $	��2,0q�6H   6o   �� 5"      -   5        $	 btnNew��5"   @   $	��New��6%   $	��2,0q�63   6P   �� 5#      -	   5        $	 	btnUpdate��5#   C   $��Update��6%   $	��2,0q�66   6V   �� 5$      -
   5        
 ��Name$
 
chkOffline��5$   J   $��Work Offline��6'   $��16��6>   $��Right=�6^    d� 	IsChecked$
 TrueY�6�   5%      &   System.Windows.Controls.DataGrid  5&      -   5          Name$ 
dGridTasks��5&   s   $��1��6#      
IsReadOnly .    CanUserResizeRows .  ��6\   Z�    Columns 5'      0  *System.Windows.Controls.DataGridTextColumn  5(   "   ,  &System.Windows.Controls.DataGridColumn   Header$ 	Task Name��65      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 6Z   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6e   �� *
taskName+  5)   "   $ Task Status��65    . $ 70 6\    6g   �� *task_status+4  .System.Windows.Controls.DataGridCheckBoxColumn  5*   "   $ Is Bug��69    . $ 40 6[    6f   �� *is_bug+  5+   "   $ Bug Type��65    . $ 60 6Y    6d   �� *
bug_type+  5,   "   $ Approved��69    . $ 60 6]    6h   �� *
approved+  5-   "   $ Target Date��65    . $ 1* 6\    6g   �� *target_date+ �� StringFormat$ {0:dd - MMM - yyyy}��  5.   "   $ Executor��65    . $ 1* 6Y    6d   �� *DeveloperName+  5/   "   $ Reviewer��65    . $ 1* 6Y    6d   �� *ReviewerName+  50   "   $ Created��65    . $ 1* 6X    6c   �� *
created_at+$ {0:d}��  51   "   $ Modified��65    . $ 1* 6Y    6d   �� *
updated_at+$ {0:d}��53      �� 54      $��Stretch=�6'   $��2��6E    �� ResizeDirection$
 Rows=�6R    �� ResizeBehavior$ PreviousAndNext=�6i   
���   ����	#FF688CAF6�   '   !Grind.WPF.CSharp.TaskTrackingForm  55      -   5          Name$ ttfrmControl��55   >   $��3��61   6>   56      57      58      �� 59      -   5        $ tbUsers��59   %   $��Users��6   .��5:      -   'Grind.WPF.CSharp.UserMaintenanceControl  -   5          Name$ userMaintenanceControl1��5:   /   5;      �� 5<      -   5        $ tbChat��5<   $   $
��Chat��6   .��5=      !	   Grind.WPF.CSharp.ChatClient	  -   5         	 Name$ ChatClientControl��5=   #   5>      5?      5@      5A      