   M S B A M L   `   `   `  ����l  hSystem.Windows.Controls.DataVisualization.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null�rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization  5   	   �{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5   	    FantasyFootballApp&    FantasyFootball.cs.OptimalWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   �DVrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   �DVC{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	     � Title$  OptimalWindow��5   	   $	��728��6   $
��1366��6,    � WindowStyle$
 None=�69    � 
ResizeMode$ NoResize=�6L   .��5      � ��5   
   �� 5	       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5
      �  � Color$ Black��6    � Offset$ 0Q�6-   � 5      $ 	#FF444343��6   $ 1Q�61   5      .+�5   
   >   8System.Windows.Controls.DataVisualization.Charting.Chart  -   5        $��mcChart��5      
������ 
�������   Title$ Points By Position��5      $��812,10,10,191q�5      $��	Meiryo UI)�6+   $��0q�6B      Palette 5      L   FSystem.Windows.Controls.DataVisualization.ResourceDictionaryCollection     DataPointStyle	 �� 
TargetType
 �� Property �� Value�� 5      %T   &	        �� 5      "	 ��67   .�5      �� 
 � ��
 �   �6;   5      5      �� 5      %T   &	        �� 5      "	 ��67   .�5      �� 
 � ��
 �����6;   5      5       5!         LegendStyle 5#      �� 5$      "	 ��6   .�5%      �� 
 � ��$ 0��6.   �� 5&      
 � ��$ 0��6/   5'         Series 5*      B   <System.Windows.Controls.DataVisualization.Charting.BarSeries  $��16(�5,   :   H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   IndependentValueBinding 5+      ��  �� Path$	 Key�   DependentValueBinding 5,      �� $ Value�5-      5/      �� 50   
   -   5         ��Name$ 	panelTeam��50      $
��Left=�6)   $	��438��6D   $��	10,10,0,0q�6Q   $	��Top=�6d   $	��466��6|    J� OpacityMask 51      �� 52      $ 0.5,1&�6&   $ 0.5,0&�67   .��53      � $ Black��6#   $ 0Q�61   � 54      $ White��6#   $ 1Q�61   55      ��57      
� 58      .��59      � $ Black��6#   $ 1Q�61   � 5:      $ 	#FF091903��6#   $ 0.444Q�65   5;      5=      �� 5>   
   $
��Left=�6   $	��496��60   $��
519,10,0,0q�6=   $	��Top=�6Q   $	��357��6i   .+�5?      �� $��Budgets:��6   $��47��6'   
�������$��	Meiryo UI)�6F   $��24(�6]   �� 5@      -   5        $��budgetStack��5@      $	��274��6,   �� 5A      -   5        $
��week��5A      $��Points Per Week:��6    $��84��6;   
�������$��	Meiryo UI)�6Z   $��24(�6q   �� 5B      -   5        $��dollar��5B      $��Points Per $$$:��6"   $��91��6<   
�������$��	Meiryo UI)�6[   $��24(�6r   5C      �� 5D   
   -   5        $
��Back��5D      $
��Left=�6    $��1161,620,0,0q�6;   $	��Top=�6Q   $	��172��6i   $��48��6u   ���	#FF1D1818
�������$��	Meiryo UI)�6�   $��18(�6�   6�   5F      5G      