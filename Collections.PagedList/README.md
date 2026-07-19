# PagedList

An immutable, read-only paginated collection. Ideal as the result type for paginated REST endpoints or any other scenario that returns paged data.

A `PagedList<T>` is a single page of items plus the metadata needed to navigate the rest of the pages: the page number, the page size, and the total number of items across every page.

## Creating a PagedList

When you already have a page slice and a separately-computed total (the typical REST/database case):

```csharp
var page = new PagedList<Product>(itemsForThisPage, pageNumber: 2, pageSize: 20, totalCount: 135);
```

When you have the full sequence in memory and want to slice it:

```csharp
var page = allProducts.ToPagedList(pageNumber: 2, pageSize: 20);
```

The empty page is available as `PagedList<T>.Empty`.

## Using a PagedList

`PagedList<T>` implements `IReadOnlyList<T>`, so it enumerates and indexes its current page like any other read-only list. The pagination metadata is exposed alongside:

```csharp
page.PageNumber;      // 2
page.PageSize;        // 20
page.TotalCount;      // 135
page.PageCount;       // 7  (derived)
page.Count;           // number of items on this page
page.HasNextPage;     // true
page.HasPreviousPage; // true
page.IsFirstPage;     // false
page.IsLastPage;      // false
```

`PagedList<T>` has value equality: two pages are equal when their items (in order) and their pagination metadata are equal.

## JSON serialization

By default `System.Text.Json` would serialize any `IEnumerable<T>` (including `PagedList<T>`) as a bare JSON array, discarding the pagination metadata. Register the included converter so it serializes as an object instead:

```csharp
var options = new JsonSerializerOptions().WithPagedListConverters();

var json = JsonSerializer.Serialize(page, options);
var roundTripped = JsonSerializer.Deserialize<PagedList<Product>>(json, options);
```

Produces:

```json
{
  "Items": [ ... ],
  "PageNumber": 2,
  "PageSize": 20,
  "TotalCount": 135,
  "PageCount": 7
}
```

The converter honours the options' `PropertyNamingPolicy` (so `JsonNamingPolicy.CamelCase` yields `items`, `pageNumber`, ...) and `PropertyNameCaseInsensitive`. `PageCount` is derived and emitted purely as a convenience for clients; it is ignored when reading.
