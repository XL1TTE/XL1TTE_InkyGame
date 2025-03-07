using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Internal.Utilities
{
    public static class SelectableNavigationUtility
    {

        /// <summary>
        /// Connect sel_one with sel_two, depends on the direction of sel_one to one_two.
        /// It's one way, that's mean that sel_two navigation will NOT be touched.
        /// </summary>
        /// <param name="sel_one">Selectable element which navigation will be connect with sel_two.</param>
        /// <param name="sel_two">Selectable element to define direction of connection.</param>
        /// <returns></returns>
        public static IEnumerator ConnectSelectablesWithDirection(Selectable sel_one, Selectable sel_two)
        {
            yield return null;

            Vector3 sel_one_pos = sel_one.gameObject.transform.position;
            Vector3 sel_two_pos = sel_one.gameObject.transform.position;

            Vector3 direction = (sel_two_pos - sel_one_pos).normalized;

            Navigation nav = sel_one.navigation;
            nav.mode = Navigation.Mode.Explicit;

            if (Vector3.Dot(direction, Vector3.up) > 0.7f)
            {
                nav.selectOnUp = sel_two;
            }
            else if (Vector3.Dot(direction, Vector3.down) > 0.7f)
            {
                nav.selectOnDown = sel_two;
            }
            else if (Vector3.Dot(direction, Vector3.left) > 0.7f)
            {
                nav.selectOnLeft = sel_two;
            }
            else if (Vector3.Dot(direction, Vector3.right) > 0.7f)
            {
                nav.selectOnRight = sel_two;
            }

            sel_one.navigation = nav;
        }

        public static IEnumerator SetupRowsLikeNavigation(List<Selectable> selectables, int maxItemsInRow)
        {
            yield return null;

            int totalItems = selectables.Count;
            int totalRows = (totalItems + maxItemsInRow - 1) / maxItemsInRow;

            for (int i = 0; i < totalItems; i++)
            {
                var currentSelectable = selectables[i];

                Navigation nav = new Navigation { mode = Navigation.Mode.Explicit };

                // Установка навигации вверх
                if (i >= maxItemsInRow)
                {
                    nav.selectOnUp = selectables[i - maxItemsInRow];
                }

                // Установка навигации вниз
                if (i < totalItems - maxItemsInRow)
                {
                    nav.selectOnDown = selectables[i + maxItemsInRow];
                }

                // Установка навигации влево
                if (i % maxItemsInRow > 0)
                {
                    nav.selectOnLeft = selectables[i - 1];
                }

                // Установка навигации вправо
                if (i % maxItemsInRow < maxItemsInRow - 1)
                {
                    nav.selectOnRight = selectables[i + 1];
                }

                // Применение настроенной навигации к текущему Selectable
                currentSelectable.navigation = nav;
            }
        }


        public static IEnumerator CategoriesLike_Sel_Navigation(List<List<GameObject>> Categories, int MaxItemsInCategory)
        {
            yield return null;

            var categories = Categories;


            for (int categoryIndex = 0; categoryIndex < categories.Count; categoryIndex++)
            {
                var category_items = categories[categoryIndex];

                int item_rows = (categories.Count + MaxItemsInCategory - 1) / MaxItemsInCategory;

                for (int i = 0; i < category_items.Count; i++)
                {

                    var current_slot = category_items[i];

                    Selectable current_selectable = current_slot.gameObject.GetComponent<Selectable>();

                    Navigation nav = new Navigation { mode = Navigation.Mode.Explicit };



                    if (categoryIndex > 0)
                    {
                        var prev_category = categories[categoryIndex - 1];
                        if (prev_category.Count > 0)
                        {
                            nav.selectOnUp = prev_category[Mathf.Min(i, categories[categoryIndex - 1].Count - 1)].GetComponent<Selectable>();
                        }
                    }
                    if (categoryIndex < Categories.Count - 1)
                    {
                        var next_category = categories[categoryIndex + 1];
                        if (next_category.Count > 0)
                        {
                            nav.selectOnDown = next_category[Mathf.Min(i, categories[categoryIndex + 1].Count - 1)].GetComponent<Selectable>();
                        }
                    }
                    if (i > 0)
                    {
                        nav.selectOnLeft = category_items[i - 1].gameObject.GetComponent<Selectable>();
                    }
                    if (i < category_items.Count - 1)
                    {
                        nav.selectOnRight = category_items[i + 1].gameObject.GetComponent<Selectable>();
                    }

                    int currentRow = i / MaxItemsInCategory;
                    int currentColumn = i % MaxItemsInCategory;

                    if (currentRow > 0)
                    {
                        int indexAbove = (currentRow - 1) * MaxItemsInCategory + currentColumn;

                        nav.selectOnUp = category_items[indexAbove].gameObject.GetComponent<Selectable>();

                    }

                    if (currentRow < item_rows - 1)
                    {
                        int indexBelow = (currentRow + 1) * MaxItemsInCategory + currentColumn;
                        if (indexBelow < category_items.Count)
                        {
                            nav.selectOnDown = category_items[indexBelow].gameObject.GetComponent<Selectable>();
                        }
                        else
                        {
                            nav.selectOnDown = category_items[category_items.Count - 1].gameObject.GetComponent<Selectable>();
                        }
                    }

                    current_selectable.navigation = nav;
                }
            }

        }
    }
}
