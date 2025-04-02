// MudanzaApp/Resources/SharedResource.cs
namespace MudanzaApp.Resources
{
    public class SharedResource
    {
        // Esta clase está vacía. Se utiliza como punto de anclaje
        // para la localización de recursos compartidos.
    }
}

// MudanzaApp/Data/LocalizationExtensions.cs


// Contenido de los archivos de recursos

// MudanzaApp/Resources/SharedResource.en.resx
/*
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="AppName" xml:space="preserve">
    <value>Moving Management</value>
  </data>
  <data name="Home" xml:space="preserve">
    <value>Home</value>
  </data>
  <data name="Dashboard" xml:space="preserve">
    <value>Dashboard</value>
  </data>
  <data name="MyMoves" xml:space="preserve">
    <value>My Moves</value>
  </data>
  <data name="CreateMove" xml:space="preserve">
    <value>Create Move</value>
  </data>
  <data name="Login" xml:space="preserve">
    <value>Login</value>
  </data>
  <data name="Register" xml:space="preserve">
    <value>Register</value>
  </data>
  <data name="Logout" xml:space="preserve">
    <value>Logout</value>
  </data>
  <data name="Welcome" xml:space="preserve">
    <value>Welcome</value>
  </data>
  <data name="Language" xml:space="preserve">
    <value>Language</value>
  </data>
  <data name="English" xml:space="preserve">
    <value>English</value>
  </data>
  <data name="Spanish" xml:space="preserve">
    <value>Spanish</value>
  </data>
  
  <!-- Mudanza Status -->
  <data name="Status_DraftOpen" xml:space="preserve">
    <value>Open Draft</value>
  </data>
  <data name="Status_DraftClosed" xml:space="preserve">
    <value>Closed Draft</value>
  </data>
  <data name="Status_InReview" xml:space="preserve">
    <value>In Review</value>
  </data>
  <data name="Status_WaitingForDocuments" xml:space="preserve">
    <value>Waiting for Documents</value>
  </data>
  <data name="Status_ReceivedInWarehouseUS" xml:space="preserve">
    <value>Received in US Warehouse</value>
  </data>
  <data name="Status_ReceivedInWarehouseMX" xml:space="preserve">
    <value>Received in MX Warehouse</value>
  </data>
  <data name="Status_WaitingForCrossing" xml:space="preserve">
    <value>Waiting for Border Crossing</value>
  </data>
  <data name="Status_WaitingForShipment" xml:space="preserve">
    <value>Waiting for Shipment</value>
  </data>
  <data name="Status_InTransit" xml:space="preserve">
    <value>In Transit</value>
  </data>
  <data name="Status_Delivered" xml:space="preserve">
    <value>Delivered</value>
  </data>
  <data name="Status_Completed" xml:space="preserve">
    <value>Completed</value>
  </data>
  
  <!-- Building Types -->
  <data name="BuildingType_House" xml:space="preserve">
    <value>House</value>
  </data>
  <data name="BuildingType_Apartment" xml:space="preserve">
    <value>Apartment</value>
  </data>
  <data name="BuildingType_Commercial" xml:space="preserve">
    <value>Commercial Property</value>
  </data>
  <data name="BuildingType_Other" xml:space="preserve">
    <value>Other</value>
  </data>
  
  <!-- Mudanza Form -->
  <data name="MoveName" xml:space="preserve">
    <value>Move Name</value>
  </data>
  <data name="CollaboratorName" xml:space="preserve">
    <value>Collaborator Name</value>
  </data>
  <data name="Origin" xml:space="preserve">
    <value>Origin</value>
  </data>
  <data name="Destination" xml:space="preserve">
    <value>Destination</value>
  </data>
  <data name="NeedsPickup" xml:space="preserve">
    <value>Needs Pickup Service</value>
  </data>
  <data name="HasStairs" xml:space="preserve">
    <value>Has Stairs</value>
  </data>
  <data name="NeedsCrane" xml:space="preserve">
    <value>Needs Crane for Heavy Items</value>
  </data>
  <data name="DirtRoad" xml:space="preserve">
    <value>Dirt Road Access</value>
  </data>
  <data name="AccessCode" xml:space="preserve">
    <value>Access Code (if needed)</value>
  </data>
  <data name="VehicleRestrictions" xml:space="preserve">
    <value>Vehicle Restrictions</value>
  </data>
  <data name="Submit" xml:space="preserve">
    <value>Submit</value>
  </data>
  <data name="Cancel" xml:space="preserve">
    <value>Cancel</value>
  </data>
  <data name="Save" xml:space="preserve">
    <value>Save</value>
  </data>
  <data name="Next" xml:space="preserve">
    <value>Next</value>
  </data>
  <data name="Back" xml:space="preserve">
    <value>Back</value>
  </data>
  
  <!-- Items -->
  <data name="Items" xml:space="preserve">
    <value>Items</value>
  </data>
  <data name="AddItem" xml:space="preserve">
    <value>Add Item</value>
  </data>
  <data name="ItemName" xml:space="preserve">
    <value>Item Name</value>
  </data>
  <data name="Category" xml:space="preserve">
    <value>Category</value>
  </data>
  <data name="Weight" xml:space="preserve">
    <value>Weight (lbs)</value>
  </data>
  <data name="IsWeightTotal" xml:space="preserve">
    <value>Is total weight (not per unit)</value>
  </data>
  <data name="IsNew" xml:space="preserve">
    <value>Item is new</value>
  </data>
  <data name="Dimensions" xml:space="preserve">
    <value>Dimensions (inches)</value>
  </data>
  <data name="Length" xml:space="preserve">
    <value>Length</value>
  </data>
  <data name="Width" xml:space="preserve">
    <value>Width</value>
  </data>
  <data name="Height" xml:space="preserve">
    <value>Height</value>
  </data>
  <data name="DeclaredValue" xml:space="preserve">
    <value>Declared Value (USD)</value>
  </data>
  <data name="IsValueTotal" xml:space="preserve">
    <value>Is total value (not per unit)</value>
  </data>
  <data name="Description" xml:space="preserve">
    <value>Description</value>
  </data>
  <data name="IsHazmat" xml:space="preserve">
    <value>Is Hazardous Material</value>
  </data>
  <data name="Quantity" xml:space="preserve">
    <value>Quantity</value>
  </data>
  <data name="UploadPhoto" xml:space="preserve">
    <value>Upload Photo</value>
  </data>
  <data name="TakePhoto" xml:space="preserve">
    <value>Take Photo (AI Analysis)</value>
  </data>
  
  <!-- Photo Credits -->
  <data name="PhotoCredits" xml:space="preserve">
    <value>Photo Credits</value>
  </data>
  <data name="AvailableCredits" xml:space="preserve">
    <value>Available Credits</value>
  </data>
  <data name="BuyCredits" xml:space="preserve">
    <value>Buy Credits</value>
  </data>
  <data name="FreeCredits" xml:space="preserve">
    <value>Free Credits</value>
  </data>
  <data name="PhotoCreditPackages" xml:space="preserve">
    <value>Photo Credit Packages</value>
  </data>
  <data name="MostPopular" xml:space="preserve">
    <value>Most Popular</value>
  </data>
  
  <!-- Collaboration -->
  <data name="ShareMove" xml:space="preserve">
    <value>Share Move</value>
  </data>
  <data name="Collaborators" xml:space="preserve">
    <value>Collaborators</value>
  </data>
  <data name="InviteCollaborator" xml:space="preserve">
    <value>Invite Collaborator</value>
  </data>
  <data name="CopyLink" xml:space="preserve">
    <value>Copy Link</value>
  </data>
  <data name="SendInvitation" xml:space="preserve">
    <value>Send Invitation</value>
  </data>
  <data name="OpenInMobile" xml:space="preserve">
    <value>Open in Mobile</value>
  </data>
  
  <!-- Dashboard -->
  <data name="RecentMoves" xml:space="preserve">
    <value>Recent Moves</value>
  </data>
  <data name="PendingReviews" xml:space="preserve">
    <value>Pending Reviews</value>
  </data>
  <data name="InProgressMoves" xml:space="preserve">
    <value>In Progress Moves</value>
  </data>
  <data name="CompletedMoves" xml:space="preserve">
    <value>Completed Moves</value>
  </data>
  <data name="AllMoves" xml:space="preserve">
    <value>All Moves</value>
  </data>
  
  <!-- Admin -->
  <data name="AdminDashboard" xml:space="preserve">
    <value>Admin Dashboard</value>
  </data>
  <data name="Users" xml:space="preserve">
    <value>Users</value>
  </data>
  <data name="Settings" xml:space="preserve">
    <value>Settings</value>
  </data>
  <data name="EstimatedCost" xml:space="preserve">
    <value>Estimated Cost</value>
  </data>
  <data name="FinalCost" xml:space="preserve">
    <value>Final Cost</value>
  </data>
  <data name="UpdateStatus" xml:space="preserve">
    <value>Update Status</value>
  </data>
  <data name="PaymentDetails" xml:space="preserve">
    <value>Payment Details</value>
  </data>
</root>
*/

// MudanzaApp/Resources/SharedResource.es.resx
/*
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="AppName" xml:space="preserve">
    <value>Gestión de Mudanzas</value>
  </data>
  <data name="Home" xml:space="preserve">
    <value>Inicio</value>
  </data>
  <data name="Dashboard" xml:space="preserve">
    <value>Panel</value>
  </data>
  <data name="MyMoves" xml:space="preserve">
    <value>Mis Mudanzas</value>
  </data>
  <data name="CreateMove" xml:space="preserve">
    <value>Crear Mudanza</value>
  </data>
  <data name="Login" xml:space="preserve">
    <value>Iniciar Sesión</value>
  </data>
  <data name="Register" xml:space="preserve">
    <value>Registrarse</value>
  </data>
  <data name="Logout" xml:space="preserve">
    <value>Cerrar Sesión</value>
  </data>
  <data name="Welcome" xml:space="preserve">
    <value>Bienvenido</value>
  </data>
  <data name="Language" xml:space="preserve">
    <value>Idioma</value>
  </data>
  <data name="English" xml:space="preserve">
    <value>Inglés</value>
  </data>
  <data name="Spanish" xml:space="preserve">
    <value>Español</value>
  </data>
  
  <!-- Mudanza Status -->
  <data name="Status_DraftOpen" xml:space="preserve">
    <value>Borrador Abierto</value>
  </data>
  <data name="Status_DraftClosed" xml:space="preserve">
    <value>Borrador Cerrado</value>
  </data>
  <data name="Status_InReview" xml:space="preserve">
    <value>En Revisión</value>
  </data>
  <data name="Status_WaitingForDocuments" xml:space="preserve">
    <value>Esperando Documentos</value>
  </data>
  <data name="Status_ReceivedInWarehouseUS" xml:space="preserve">
    <value>Recibido en Almacén EEUU</value>
  </data>
  <data name="Status_ReceivedInWarehouseMX" xml:space="preserve">
    <value>Recibido en Almacén MX</value>
  </data>
  <data name="Status_WaitingForCrossing" xml:space="preserve">
    <value>Esperando para Cruce</value>
  </data>
  <data name="Status_WaitingForShipment" xml:space="preserve">
    <value>Esperando para Envío</value>
  </data>
  <data name="Status_InTransit" xml:space="preserve">
    <value>En Tránsito</value>
  </data>
  <data name="Status_Delivered" xml:space="preserve">
    <value>Entregado</value>
  </data>
  <data name="Status_Completed" xml:space="preserve">
    <value>Completado</value>
  </data>
  
  <!-- Building Types -->
  <data name="BuildingType_House" xml:space="preserve">
    <value>Casa</value>
  </data>
  <data name="BuildingType_Apartment" xml:space="preserve">
    <value>Departamento</value>
  </data>
  <data name="BuildingType_Commercial" xml:space="preserve">
    <value>Local Comercial</value>
  </data>
  <data name="BuildingType_Other" xml:space="preserve">
    <value>Otro</value>
  </data>
  
  <!-- Mudanza Form -->
  <data name="MoveName" xml:space="preserve">
    <value>Nombre de la Mudanza</value>
  </data>
  <data name="CollaboratorName" xml:space="preserve">
    <value>Nombre del Colaborador</value>
  </data>
  <data name="Origin" xml:space="preserve">
    <value>Origen</value>
  </data>
  <data name="Destination" xml:space="preserve">
    <value>Destino</value>
  </data>
  <data name="NeedsPickup" xml:space="preserve">
    <value>Necesita Servicio de Recolección</value>
  </data>
  <data name="HasStairs" xml:space="preserve">
    <value>Tiene Escaleras</value>
  </data>
  <data name="NeedsCrane" xml:space="preserve">
    <value>Necesita Grúa para Artículos Pesados</value>
  </data>
  <data name="DirtRoad" xml:space="preserve">
    <value>Acceso por Terracería</value>
  </data>
  <data name="AccessCode" xml:space="preserve">
    <value>Código de Acceso (si es necesario)</value>
  </data>
  <data name="VehicleRestrictions" xml:space="preserve">
    <value>Restricciones de Vehículos</value>
  </data>
  <data name="Submit" xml:space="preserve">
    <value>Enviar</value>
  </data>
  <data name="Cancel" xml:space="preserve">
    <value>Cancelar</value>
  </data>
  <data name="Save" xml:space="preserve">
    <value>Guardar</value>
  </data>
  <data name="Next" xml:space="preserve">
    <value>Siguiente</value>
  </data>
  <data name="Back" xml:space="preserve">
    <value>Atrás</value>
  </data>
  
  <!-- Items -->
  <data name="Items" xml:space="preserve">
    <value>Artículos</value>
  </data>
  <data name="AddItem" xml:space="preserve">
    <value>Agregar Artículo</value>
  </data>
  <data name="ItemName" xml:space="preserve">
    <value>Nombre del Artículo</value>
  </data>
  <data name="Category" xml:space="preserve">
    <value>Categoría</value>
  </data>
  <data name="Weight" xml:space="preserve">
    <value>Peso (libras)</value>
  </data>
  <data name="IsWeightTotal" xml:space="preserve">
    <value>Es peso total (no por unidad)</value>
  </data>
  <data name="IsNew" xml:space="preserve">
    <value>Artículo nuevo</value>
  </data>
  <data name="Dimensions" xml:space="preserve">
    <value>Dimensiones (pulgadas)</value>
  </data>
  <data name="Length" xml:space="preserve">
    <value>Largo</value>
  </data>
  <data name="Width" xml:space="preserve">
    <value>Ancho</value>
  </data>
  <data name="Height" xml:space="preserve">
    <value>Alto</value>
  </data>
  <data name="DeclaredValue" xml:space="preserve">
    <value>Valor Declarado (USD)</value>
  </data>
  <data name="IsValueTotal" xml:space="preserve">
    <value>Es valor total (no por unidad)</value>
  </data>
  <data name="Description" xml:space="preserve">
    <value>Descripción</value>
  </data>
  <data name="IsHazmat" xml:space="preserve">
    <value>Es Material Peligroso</value>
  </data>
  <data name="Quantity" xml:space="preserve">
    <value>Cantidad</value>
  </data>
  <data name="UploadPhoto" xml:space="preserve">
    <value>Subir Foto</value>
  </data>
  <data name="TakePhoto" xml:space="preserve">
    <value>Tomar Foto (Análisis IA)</value>
  </data>
  
  <!-- Photo Credits -->
  <data name="PhotoCredits" xml:space="preserve">
    <value>Créditos de Fotos</value>
  </data>
  <data name="AvailableCredits" xml:space="preserve">
    <value>Créditos Disponibles</value>
  </data>
  <data name="BuyCredits" xml:space="preserve">
    <value>Comprar Créditos</value>
  </data>
  <data name="FreeCredits" xml:space="preserve">
    <value>Créditos Gratis</value>
  </data>
  <data name="PhotoCreditPackages" xml:space="preserve">
    <value>Paquetes de Créditos de Fotos</value>
  </data>
  <data name="MostPopular" xml:space="preserve">
    <value>Más Popular</value>
  </data>
  
  <!-- Collaboration -->
  <data name="ShareMove" xml:space="preserve">
    <value>Compartir Mudanza</value>
  </data>
  <data name="Collaborators" xml:space="preserve">
    <value>Colaboradores</value>
  </data>
  <data name="InviteCollaborator" xml:space="preserve">
    <value>Invitar Colaborador</value>
  </data>
  <data name="CopyLink" xml:space="preserve">
    <value>Copiar Enlace</value>
  </data>
  <data name="SendInvitation" xml:space="preserve">
    <value>Enviar Invitación</value>
  </data>
  <data name="OpenInMobile" xml:space="preserve">
    <value>Abrir en Móvil</value>
  </data>
  
  <!-- Dashboard -->
  <data name="RecentMoves" xml:space="preserve">
    <value>Mudanzas Recientes</value>
  </data>
  <data name="PendingReviews" xml:space="preserve">
    <value>Revisiones Pendientes</value>
  </data>
  <data name="InProgressMoves" xml:space="preserve">
    <value>Mudanzas en Progreso</value>
  </data>
  <data name="CompletedMoves" xml:space="preserve">
    <value>Mudanzas Completadas</value>
  </data>
  <data name="AllMoves" xml:space="preserve">
    <value>Todas las Mudanzas</value>
  </data>
  
  <!-- Admin -->
  <data name="AdminDashboard" xml:space="preserve">
    <value>Panel de Administración</value>
  </data>
  <data name="Users" xml:space="preserve">
    <value>Usuarios</value>
  </data>
  <data name="Settings" xml:space="preserve">
    <value>Configuración</value>
  </data>
  <data name="EstimatedCost" xml:space="preserve">
    <value>Costo Estimado</value>
  </data>
  <data name="FinalCost" xml:space="preserve">
    <value>Costo Final</value>
  </data>
  <data name="UpdateStatus" xml:space="preserve">
    <value>Actualizar Estado</value>
  </data>
  <data name="PaymentDetails" xml:space="preserve">
    <value>Detalles de Pago</value>
  </data>
</root>
*/