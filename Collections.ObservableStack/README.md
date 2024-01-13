![ObservableStack](https://github.com/Moreault/Collections/blob/master/observablestack.png)
# ObservableStack
A stack that notifies when it changes.

## Overview
`ObservableStack<T>` is used in much the same way as the standard `Stack<T>` class. The main difference is that `ObservableStack<T>` raises events when it is modified. This allows you to be notified when items are pushed or popped from the stack.

It also uses value equality and overrides `ToString` with its type name and amount of items, like most other collections in the ToolBX.Collections library.