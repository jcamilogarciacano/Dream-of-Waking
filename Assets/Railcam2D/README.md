# Railcam2D

Copyright © 2020 Jonathan Madelaine

---

## Documentation

You can find the full documentation at [railcam2d.com/docs](https://railcam2d.com/docs).

---

## Support

If you have a question or want to report a bug, please contact jonathan@jonathanmadelaine.com.

---

## Review

If you like Railcam 2D, [don't forget to review](https://assetstore.unity.com/packages/tools/camera/railcam-2d-103883#reviews).

---

## Quick start

This quick start guide covers initial setup, basic use, and core concepts.

### Importing the Package

To use Railcam 2D in a project, add the Railcam 2D Asset to the project's Assets folder.

If the Railcam 2D Asset package is stored locally, import the Asset using the [Import Custom Package](https://docs.unity3d.com/Manual/AssetPackages.html#ImportingPackages) dialog.

To import the Asset from the Unity Asset Store, use the [Download Manager](https://docs.unity3d.com/Manual/AssetStore.html#downloadmgr) in the Asset Store window of the Unity Editor.

### Adding to a Scene

Setting up Railcam 2D is quick and easy.

Simply drag the [Railcam 2D component](/docs/user-guide/railcam-2d-component) file from the Asset's root directory and drop it onto your camera.

Alterantively, use the "Add Component" button in the Inspector and search for _Railcam 2D_.

This is the only component you'll need to manually add to your scene. All other components can be added through the Railcam 2D component.

> If you already have a component that moves the camera, you can use that instead of the Railcam 2D component. See [Custom Component Integration](/docs/user-guide/custom-component-integration) for details.

### Moving the Camera

The Railcam 2D component needs a target to follow.

Create a new target by clicking the _"+"_ button on the `Targets` list, and drag your target game object (usually the player controlled character) into the `Transform` field of the newly created target.

The camera will now follow this target around the scene.

Railcam 2D allows any number of targets. See [Working with Targets](https://railcam2d.com/docs/user-guide/working-with-targets) for more information.

### Creating a Rail

Railcam 2D uses [Rail](https://railcam2d.com/docs/api-reference/rail) components to control camera movement.

To start using Rails, add a [Rail Manager](https://railcam2d.com/docs/user-guide/rail-manager-component) to the scene by clicking the _"Enable Rail Editing"_ button on the Railcam 2D component.

> If you've replaced the Railcam 2D component with a custom component, you can add the Rail Manager by either dragging the `RailManager.cs` component file from Railcam 2D's root directory and dropping it on your component, or using the "Add Component" button in the Inspector.

The Rail Manager makes working with Rails easy, as it allows the creating and editing of all Rails through a single interface.

Click the "New Rail" button in the Rail Manager to add a new Rail to the scene.

Each Rail is added to the scene as a new object with a [Rail](https://railcam2d.com/docs/api-reference/rail) component attached.

Clicking the "Show" button will center the Scene View on the Rail.

The Rail Manager displays information about the Rail, including a list of the Rail's Waypoints. These Waypoints determine the Rail's path. Each Waypoint can be moved by setting its `Position` property or by dragging the Waypoint's handle in the Scene View.

The camera will now be positioned on the Rail.

### Waypoints

A Rail is defined by its [Waypoints](https://railcam2d.com/docs/api-reference/waypoint).

Waypoints can be added by clicking the "+" button or removed by clicking the "-" button on the `Waypoints` list. This can also be done through the Scene View using the Rail's [Scene UI](https://railcam2d.com/docs/user-guide/rail-component#scene_ui).

You can think of a Rail as being made up of individual segments, where each Waypoint defines the start of a new segment. A Waypoint has several properties that effect camera movement along that Waypoint's segment.

> A Waypoint defines how the camera moves on the segment of the Rail that follows the Waypoint, therefore the final Waypoint's properties have no effect on camera movement.

The [`FollowAxis`](https://railcam2d.com/docs/api-reference/waypoint#followaxis) property determines the axis used when tracking a target. By default this is set to `XY`, which positions the camera at the closest point on the Rail to the target. Using a single axis (`X` or `Y`) aligns the camera horizontally (`X`) or vertically (`Y`) with the target.

The [`CurveType`](https://railcam2d.com/docs/api-reference/waypoint#curvetype) property determines whether the segment is a straight line (`Linear`) or a quadratic bezier curve (`Quadratic`). If set to `Quadratic`, a control point can be positioned to adjust the curve.

> Defaults for `FollowAxis` and `CurveType` can be changed in [Settings](https://railcam2d.com/docs/user-guide/settings).

As each Waypoint is independent, a Rail can have multiple Follow Axes and Curve Types.

### Effects

[Effects](https://railcam2d.com/docs/api-reference/effect) change the normal position of the camera by displacing it along the Rail.

Each Rail has its own list of Effects, which can be edited by switching from the Waypoints tab to the Effects tab in the Rail Manager.

An Effect targets a single Waypoint, and sets values for `CameraPosition` and `TargetPosition`. See [Working with Effects](https://railcam2d.com/docs/user-guide/working-with-effects) for more information.

### More Rails

There's no limit to the number of Rails you can add to a scene.

In the Rail Manager, clicking the "New Rail" button will add another game object with a Rail component, while clicking the "Delete" button will remove the currently selected Rail and its game object from the scene.

To edit a different Rail, switch to it using the dropdown at the top of the Rail Manager.

When a scene has more than one Rail, the camera will be positioned on the closest Rail to the target, and will switch between Rails automatically.

> Rails can be made inactive by setting their `Active` property to `false`, which will prevent them being used in camera position calculations.

---

## VERSION LOG

- 2.0.0 -- 2020-03-09

- 1.0.0 -- 2017-11-01
