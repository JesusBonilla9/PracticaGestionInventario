# Revisión de Práctica: Gestión de Inventario

**Revisor:** Angel Raonel Guerrero Antigua 2023-0047

## 1. Comentarios sobre el funcionamiento
El sistema presenta una interfaz adecuada.

* **Áreas de Mejora:** Entrada index deberia tener un foot, ya que visualmente no es notable.

## 2. Confirmación de Lógica de Inventario
Se han realizado pruebas unitarias sobre (Crear, Editar y Eliminar), obteniendo los siguientes resultados:

| Funcionalidad | Estado | Observaciones |
| :--- | :--- | :--- |
| **Crear Producto** | ✅ Funciona | El producto se añade correctamente a la lista. |
| **Editar Producto** | ✅ Funciona | Los cambios de precio y Existencia se reflejan de inmediato. |
| **Eliminar Producto** | ✅ Funciona | El registro se remueve sin afectar la integridad de otros datos. |
| **Eliminar Producto con entrada** | ⚠️ Advertencia | Al eliminar un producto, si vamos a editar la entrada el programa colapsa|

---
**Conclusión:** La lógica de negocio cumple con los requisitos mínimos establecidos para la gestión de inventario.
