   M S B A M L   `   `   `  ����  Grind.WPF.CSharp#    Grind.WPF.CSharp.ChatsControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5          >�Name$  UserControl��5      .��5      � -   5         �Name$ 
LayoutRoot��5      .+�5   
   �  � ColumnDefinitions 5      �� 5      $��2* �6#   �� 5      $��3 �6#   �� 5      $	��120 �6#   .+�5      �� $��1��6   $��Stretch=�6,   $��Stretch=�6H    �� ResizeBehavior$ PreviousAndNext=�6f   ���	#FF007ACC6�   � 5      $��Stretch=�6   $��Stretch=�62    � RowDefinitions 5      �� 5      $��3* �6$   �� 5      $��3 �6$   �� 5      $	��100 �6$   .+�5      �� $��Stretch=�6   $��1��6?   $��Stretch=�6L   $ PreviousAndNext=�6h   ���	#FF007ACC �� ResizeDirection$
 Rows=�6�   � 5      $��Stretch=�6   $��2��65   $��Stretch=�6B    5      �� 5      $��1* �6(   �� 5      $��25 �6(   .+�5       �� -   5         ��Name$ 
txtMessage��5        �� AcceptsReturn . " �� VerticalScrollBarVisibility$
 Auto=�6H   $��0��6k   6x   �� 5!      $��1��6!   	 �� Orientation$	 
Horizontal=�6.   .+�5$      �� -   5        
 ��Name$
 btnConn��5$      $
��Left=�6.   $��Connect��6I   6[   �� 5%      -   5        $
 btnSend��5%      $��Center=�6/   $
��Send��6L   6[   �� 5&      -   5        $
 btnSpam��5&      $
��Spam��6.   $y�	Collapsed=�6S   �� 5'      -   5        $
 btnDisc��5'      $��Right=�6/   $��
Disconnect��6L   6a   � 5)      -   5         �Name$ ppSend��5)       � 	StaysOpen .   � PlacementTarget 6,   ��  �� ElementName$ btnSend��.��5*      � ���	#FFE5E5E5 5+      �� 5,   "   �� 5-   "   $��1* �60   �� 5.   "   $��1* �60   �� 5/   "   $��1* �60   .+�51      m� -	   5         m�Name$ 
mniTargets��51   '   $��0��6;   m� 52      m� 53      -
   5        $ mniPersonalMsg��53   '   $��1��6?   $.��(Send as Personal Message to above People��6L   6~   m� 54      -   5        $ mniPublicMsg��54   '   $��2��6?   $(��"Send as Public Message to Everyone��6L   6x   55      56      57      58      �� 59      -   5         ��Name$ rtbMessages��59       �� 
IsReadOnly . $
 Auto=�6E   $ �� HorizontalScrollBarVisibility$
 Auto=�6h   . �5:      /� 5;      5<      5S      �� 5T      -   5         ��Name$ lbOnlinePeople��5T      $��2��6.      DataTemplate1#���   6>    �� SelectionMode$ Extended=�6m    � 	Resources 5V      ��%�   &	        �� 5W      .R�5X      � .+�5Y      �� $
��Bold"�6E   ��6(   �� *person_name+6E   �� 5Z      $�� (��6(   �� 5[      ��6(   �� *person_trigram+�� 5\      $��)��6(   5]      5^      5_       � ContextMenu 5`      �� 5a      .@�5b      m� -   5        $ 
mbiSendMsg��5b   #   $��Send Message��67   6M   5c      5e      5g      5h      5i      