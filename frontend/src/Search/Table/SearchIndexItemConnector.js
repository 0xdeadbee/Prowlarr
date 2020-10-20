import PropTypes from 'prop-types';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import { createSelector } from 'reselect';
import { executeCommand } from 'Store/Actions/commandActions';

function createReleaseSelector() {
  return createSelector(
    (state, { guid }) => guid,
    (state) => state.releases.items,
    (guid, releases) => {
      return releases.find((t) => t.guid === guid);
    }
  );
}

function createMapStateToProps() {
  return createSelector(
    createReleaseSelector(),
    (
      movie
    ) => {

      // If a movie is deleted this selector may fire before the parent
      // selecors, which will result in an undefined movie, if that happens
      // we want to return early here and again in the render function to avoid
      // trying to show a movie that has no information available.

      if (!movie) {
        return {};
      }

      return {
        ...movie
      };
    }
  );
}

const mapDispatchToProps = {
  dispatchExecuteCommand: executeCommand
};

class MovieIndexItemConnector extends Component {

  //
  // Render

  render() {
    const {
      guid,
      component: ItemComponent,
      ...otherProps
    } = this.props;

    if (!guid) {
      return null;
    }

    return (
      <ItemComponent
        {...otherProps}
        guid={guid}
      />
    );
  }
}

MovieIndexItemConnector.propTypes = {
  guid: PropTypes.string,
  component: PropTypes.elementType.isRequired
};

export default connect(createMapStateToProps, mapDispatchToProps)(MovieIndexItemConnector);
